using Allie.Chat.Lib.Enums;
using Allie.Chat.Lib.Interfaces;
using Allie.Chat.Lib.Structs;
using Allie.Chat.Tcp.Events;
using Newtonsoft.Json;
using PHS.Core.Enums;
using PHS.Core.Events;
using PHS.Core.Models;
using System;
using System.Threading.Tasks;
using Tcp.NET.Client.SSL;
using Tcp.NET.Core.SSL.Events.Args;

namespace Allie.Chat.Tcp
{
    public class TcpClientSSLAC : ITcpClientSSLAC
    {
        private readonly string _connectUri;
        private readonly int _connectPort;
        private readonly string _accessToken;
        private readonly string _certificateIssuedTo;

        protected readonly ITcpNETClientSSL _tcpClient;

        public event NetworkingEventHandler<TcpSSLConnectionEventArgs> ConnectionEvent;
        public event TcpMessageEventHandler<IMessageBase> MessageEvent;
        public event TcpMessageEventHandler<IMessageTwitch> MessageTwitchEvent;
        public event TcpMessageEventHandler<IMessageDiscord> MessageDiscordEvent;
        public event TcpMessageEventHandler<IMessageTcp> MessageTcpEvent;
        public event TcpMessageEventHandler<IMessageWS> MessageWebsocketEvent;
        public event NetworkingEventHandler<TcpSSLErrorEventArgs> ErrorEvent;
        public event SystemMessageEventHandler SystemMessageEvent;

        public TcpClientSSLAC(string accessToken, string url = "connect.allie.chat", int port = 7615, string certificateIssuedTo = "connect.allie.chat")
        {
            _accessToken = accessToken;
            _connectUri = url;
            _connectPort = port;
            _certificateIssuedTo = certificateIssuedTo;

            _tcpClient = new TcpNETClientSSL();
            _tcpClient.ConnectionEvent += OnConnectionEvent;
            _tcpClient.MessageEvent += OnMessageEvent;
            _tcpClient.ErrorEvent += OnErrorEvent;
        }

        public virtual async Task<bool> ConnectAsync()
        {
            if (_tcpClient.IsRunning)
            {
                _tcpClient.Disconnect();
            }

            _tcpClient.Connect(_connectUri, _connectPort, "\r\n", _certificateIssuedTo);

            if (_tcpClient.IsRunning)
            {
                await _tcpClient.SendToServerAsync($"oauth:{_accessToken}");
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
        public virtual async Task<bool> SendAsync(string message)
        {
            try
            {
                return await _tcpClient.SendToServerAsync(new PacketDTO
                {
                    Action = (int)ActionType.SendToServer,
                    Data = message,
                    Timestamp = DateTime.UtcNow
                });
            }
            catch
            { }

            return false;
        }

        protected virtual Task OnConnectionEvent(object sender, TcpSSLConnectionEventArgs args)
        {
            ConnectionEvent(sender, args);
            return Task.CompletedTask;
        }
        protected virtual async Task OnMessageEvent(object sender, TcpSSLMessageEventArgs args)
        {
            switch (args.MessageEventType)
            {
                case MessageEventType.Sent:
                    break;
                case MessageEventType.Receive:
                    if (args.Message.Trim().ToLower() == "ping")
                    {
                        await _tcpClient.SendToServerAsync("pong");
                    }
                    else
                    {
                        try
                        {
                            var message = JsonConvert.DeserializeObject<MessageBase>(args.Packet.Data);
                            IMessageBase messageTyped = null;

                            switch (message.ProviderType)
                            {
                                case ProviderType.Twitch:
                                    messageTyped = JsonConvert.DeserializeObject<MessageTwitch>(args.Packet.Data);
                                    MessageTwitchEvent?.Invoke(sender, messageTyped as IMessageTwitch);
                                    break;
                                case ProviderType.Discord:
                                    messageTyped = JsonConvert.DeserializeObject<MessageDiscord>(args.Packet.Data);
                                    MessageDiscordEvent?.Invoke(sender, messageTyped as IMessageDiscord);
                                    break;
                                case ProviderType.Tcp:
                                    messageTyped = JsonConvert.DeserializeObject<MessageTcp>(args.Packet.Data);
                                    MessageTcpEvent?.Invoke(sender, messageTyped as IMessageTcp);
                                    break;
                                case ProviderType.Websocket:
                                    messageTyped = JsonConvert.DeserializeObject<MessageWS>(args.Packet.Data);
                                    MessageWebsocketEvent?.Invoke(sender, messageTyped as IMessageWS);
                                    break;
                                default:
                                    break;
                            }

                            if (messageTyped != null)
                            {
                                MessageEvent?.Invoke(sender, messageTyped);
                            }
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
        }
        protected virtual Task OnErrorEvent(object sender, TcpSSLErrorEventArgs args)
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