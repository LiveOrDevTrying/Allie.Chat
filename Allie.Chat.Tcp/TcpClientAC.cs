using Allie.Chat.Lib.Enums;
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
    public delegate void SystemMessageEventHandler(object sender, string message);

    public class TcpClientAC : ITcpClientAC
    {
        private readonly string _connectUri = "connect.allie.chat";
        private readonly int _connectPort = 7610;
        private readonly string _accessToken;

        protected readonly ITcpNETClient _tcpClient;

        public event NetworkingEventHandler<TcpConnectionEventArgs> ConnectionEvent;
        public event TcpMessageEventHandler<IMessageBase> MessageEvent;
        public event TcpMessageEventHandler<IMessageTwitch> MessageTwitchEvent;
        public event TcpMessageEventHandler<IMessageDiscord> MessageDiscordEvent;
        public event TcpMessageEventHandler<IMessageTcp> MessageTcpEvent;
        public event TcpMessageEventHandler<IMessageWS> MessageWebsocketEvent;
        public event NetworkingEventHandler<TcpErrorEventArgs> ErrorEvent;
        public event SystemMessageEventHandler SystemMessageEvent;

        public TcpClientAC(string accessToken)
        {
            _accessToken = accessToken;

            _tcpClient = new TcpNETClient();
            _tcpClient.ConnectionEvent += OnConnectionEvent;
            _tcpClient.MessageEvent += OnMessageEvent;
            _tcpClient.ErrorEvent += OnErrorEvent;
           
        }
        public TcpClientAC(string url, int port, string accessToken)
        {
            _connectUri = url;
            _connectPort = port;
            _accessToken = accessToken;

            _tcpClient = new TcpNETClient();
            _tcpClient.ConnectionEvent += OnConnectionEvent;
            _tcpClient.MessageEvent += OnMessageEvent;
            _tcpClient.ErrorEvent += OnErrorEvent;
        }

        public virtual bool Connect()
        {
            if (_tcpClient.IsRunning)
            {
                _tcpClient.Disconnect();
            }

            _tcpClient.Connect(_connectUri, _connectPort, "\r\n");

            if (_tcpClient.IsRunning)
            {
                _tcpClient.SendToServer($"oauth:{_accessToken}");
                return true;
            }

            return false;
        }
        public virtual bool Disconnect()
        {
            if (_tcpClient.IsRunning)
            {
                _tcpClient.Disconnect();
                return true;
            }

            return false;
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
                        _tcpClient.SendToServer("pong");
                    }
                    else
                    {
                        try
                        {
                            var message = JsonConvert.DeserializeObject<MessageBase>(args.Packet.Data);

                            switch (message.ProviderType)
                            {
                                case ProviderType.Twitch:
                                    MessageTwitchEvent?.Invoke(sender, JsonConvert.DeserializeObject<MessageTwitch>(args.Packet.Data));
                                    break;
                                case ProviderType.Discord:
                                    MessageDiscordEvent?.Invoke(sender, JsonConvert.DeserializeObject<MessageDiscord>(args.Packet.Data));
                                    break;
                                case ProviderType.Tcp:
                                    MessageTcpEvent?.Invoke(sender, JsonConvert.DeserializeObject<MessageTcp>(args.Packet.Data));
                                    break;
                                case ProviderType.Websocket:
                                    MessageWebsocketEvent?.Invoke(sender, JsonConvert.DeserializeObject<MessageWS>(args.Packet.Data));
                                    break;
                                default:
                                    break;
                            }

                            MessageEvent?.Invoke(sender, message);
                        }
                        catch
                        {
                            SystemMessageEvent?.Invoke(sender, args.Message);
                        }
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
                return _tcpClient != null && _tcpClient.IsRunning;
            }
        }
    }
}