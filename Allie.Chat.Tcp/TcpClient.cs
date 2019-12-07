﻿using Allie.Chat.Lib.Enums;
using Allie.Chat.Lib.Interfaces;
using Allie.Chat.Lib.Structs;
using Newtonsoft.Json;
using PHS.Core.Enums;
using PHS.Core.Events;
using PHS.Core.Models;
using System;
using System.Threading.Tasks;
using Tcp.NET.Client;
using Tcp.NET.Core.Events.Args;

namespace Allie.Chat.Tcp
{
    public delegate void TcpMessageEventHandler<T>(object sender, T args) where T : IMessageBase;

    public class TcpClient : ITcpClient
    {
        private readonly string _connectUri = "https://connect.allie.chat";
        private readonly int _connectPort = 7415;
        protected readonly ITcpAsyncClient _tcpClient;

        public event NetworkingEventHandler<TcpConnectionEventArgs> ConnectionEvent;
        public event TcpMessageEventHandler<IMessageBase> MessageEvent;
        public event TcpMessageEventHandler<IMessageTwitch> MessageTwitchEvent;
        public event TcpMessageEventHandler<IMessageDiscord> MessageDiscordEvent;
        public event TcpMessageEventHandler<IMessageTcp> MessageTcpEvent;
        public event TcpMessageEventHandler<IMessageWS> MessageWebsocketEvent;
        public event NetworkingEventHandler<TcpErrorEventArgs> ErrorEvent;

        public TcpClient(string accessToken)
        {
            _tcpClient = new TcpAsyncClient();
            _tcpClient.ConnectionEvent += OnConnectionEvent;
            _tcpClient.MessageEvent += OnMessageEvent;
            _tcpClient.ErrorEvent += OnErrorEvent;
            _tcpClient.Start(_connectUri, _connectPort, "/r/n");

            if (_tcpClient.IsConnected)
            {
                _tcpClient.SendToServerRaw($"oauth:{accessToken}");
            }
        }
        public TcpClient(string url, int port, string accessToken)
        {
            _connectUri = url;
            _connectPort = port;

            _tcpClient = new TcpAsyncClient();
            _tcpClient.ConnectionEvent += OnConnectionEvent;
            _tcpClient.MessageEvent += OnMessageEvent;
            _tcpClient.ErrorEvent += OnErrorEvent;
            _tcpClient.Start(_connectUri, _connectPort, "/r/n");

            if (_tcpClient.IsConnected)
            {
                _tcpClient.SendToServerRaw($"oauth:{accessToken}");
            }
        }

        public virtual Task<bool> SendAsync(string message)
        {
            var response = _tcpClient.SendToServer(new PacketDTO
            {
                Action = (int)ActionType.SendToServer,
                Data = message,
                Timestamp = DateTime.UtcNow
            });

            return Task.FromResult(response);
        }

        protected virtual Task OnConnectionEvent(object sender, TcpConnectionEventArgs args)
        {
            ConnectionEvent(sender, args);
            return Task.CompletedTask;
        }
        protected virtual Task OnMessageEvent(object sender, TcpMessageEventArgs args)
        {
            switch (args.MessageEventType)
            {
                case MessageEventType.Sent:
                    break;
                case MessageEventType.Receive:
                    if (args.Message.Trim().ToLower() == "ping")
                    {
                        _tcpClient.SendToServerRaw("pong");
                    }
                    else
                    {
                        try
                        {
                            var message = JsonConvert.DeserializeObject<MessageBase>(args.Packet.Data);

                            switch (message.ProviderType)
                            {
                                case ProviderType.Twitch:
                                    MessageTwitchEvent(sender, JsonConvert.DeserializeObject<MessageTwitch>(args.Packet.Data));
                                    break;
                                case ProviderType.Discord:
                                    MessageDiscordEvent(sender, JsonConvert.DeserializeObject<MessageDiscord>(args.Packet.Data));
                                    break;
                                case ProviderType.Tcp:
                                    MessageTcpEvent(sender, JsonConvert.DeserializeObject<MessageTcp>(args.Packet.Data));
                                    break;
                                case ProviderType.Websocket:
                                    MessageWebsocketEvent(sender, JsonConvert.DeserializeObject<MessageWS>(args.Packet.Data));
                                    break;
                                default:
                                    break;
                            }

                            MessageEvent(sender, message);
                        }
                        catch
                        { }
                    }
                    break;
                default:
                    break;
            }
            return Task.CompletedTask;
        }
        protected virtual Task OnErrorEvent(object sender, TcpErrorEventArgs args)
        {
            ErrorEvent(sender, args);
            return Task.CompletedTask;
        }

        public virtual void Dispose()
        {
            if (_tcpClient != null)
            {
                _tcpClient.ConnectionEvent -= OnConnectionEvent;
                _tcpClient.MessageEvent -= OnMessageEvent;
                _tcpClient.ErrorEvent -= OnErrorEvent;
                _tcpClient.Dispose();
            }
        }

        public bool IsRunning
        {
            get
            {
                return _tcpClient != null && _tcpClient.IsConnected;
            }
        }
    }
}