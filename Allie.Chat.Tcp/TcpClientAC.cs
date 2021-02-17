using Allie.Chat.Lib.Enums;
using Allie.Chat.Lib.Interfaces;
using Allie.Chat.Lib.Structs;
using Allie.Chat.Tcp.Events;
using Newtonsoft.Json;
using PHS.Networking.Enums;
using PHS.Networking.Events;
using PHS.Networking.Models;
using System;
using System.Threading.Tasks;
using Tcp.NET.Client;
using Tcp.NET.Client.Events.Args;
using Tcp.NET.Client.Models;

namespace Allie.Chat.Tcp
{
    public class TcpClientAC : ITcpClientAC
    {
        private readonly string _connectUri;
        private readonly int _connectPort;
        private readonly string _accessToken;

        protected readonly ITcpNETClient _tcpClient;

        public event NetworkingEventHandler<TcpConnectionClientEventArgs> ConnectionEvent;
        public event TcpMessageEventHandler<IMessageBase> MessageEvent;
        public event TcpMessageEventHandler<IMessageTwitch> MessageTwitchEvent;
        public event TcpMessageEventHandler<IMessageDiscord> MessageDiscordEvent;
        public event TcpMessageEventHandler<IMessageTcp> MessageTcpEvent;
        public event TcpMessageEventHandler<IMessageWS> MessageWebsocketEvent;
        public event NetworkingEventHandler<TcpErrorClientEventArgs> ErrorEvent;
        public event SystemMessageEventHandler SystemMessageEvent;

        public TcpClientAC(string accessToken, string url = "connect.allie.chat", int port = 7625, bool isSSL = true)
        {
            _accessToken = accessToken;
            _connectUri = url;
            _connectPort = port;

            _tcpClient = new TcpNETClient(new ParamsTcpClient
            {
                EndOfLineCharacters = "\r\n",
                IsSSL = isSSL,
                Port = _connectPort,
                Uri = _connectUri
            }, _accessToken);
            _tcpClient.ConnectionEvent += OnConnectionEvent;
            _tcpClient.MessageEvent += OnMessageEvent;
            _tcpClient.ErrorEvent += OnErrorEvent;
        }

        public virtual async Task<bool> ConnectAsync()
        {
            try
            {
                if (_tcpClient.IsRunning)
                {
                    await _tcpClient.DisconnectAsync();
                }

                await _tcpClient.ConnectAsync();

                return true;
            }
            catch
            { }

            return false;
        }
        public virtual async Task<bool> DisconnectAsync()
        {
            if (_tcpClient.IsRunning)
            {
                await _tcpClient.DisconnectAsync();
                return true;
            }

            return false;
        }
        public virtual async Task<bool> SendAsync(string message)
        {
            return await _tcpClient.SendToServerAsync(new Packet
            {
                Data = message,
                Timestamp = DateTime.UtcNow
            });
        }

        protected virtual async Task OnConnectionEvent(object sender, TcpConnectionClientEventArgs args)
        {
            if (ConnectionEvent != null)
            {
                await ConnectionEvent?.Invoke(sender, args);
            }
        }
        protected virtual async Task OnMessageEvent(object sender, TcpMessageClientEventArgs args)
        {
            switch (args.MessageEventType)
            {
                case MessageEventType.Sent:
                    break;
                case MessageEventType.Receive:
                    try
                    {
                        var message = JsonConvert.DeserializeObject<MessageBase>(args.Packet.Data);
                        IMessageBase messageTyped = null;

                        switch (message.ProviderType)
                        {
                            case ProviderType.Twitch:
                                messageTyped = JsonConvert.DeserializeObject<MessageTwitch>(args.Packet.Data);

                                if (MessageTwitchEvent != null)
                                {
                                    await MessageTwitchEvent?.Invoke(sender, messageTyped as IMessageTwitch);
                                }
                                break;
                            case ProviderType.Discord:
                                messageTyped = JsonConvert.DeserializeObject<MessageDiscord>(args.Packet.Data);
                                
                                if (MessageDiscordEvent != null)
                                {
                                    await MessageDiscordEvent?.Invoke(sender, messageTyped as IMessageDiscord);
                                }
                                break;
                            case ProviderType.Tcp:
                                messageTyped = JsonConvert.DeserializeObject<MessageTcp>(args.Packet.Data);

                                if (MessageTcpEvent != null)
                                {
                                    await MessageTcpEvent?.Invoke(sender, messageTyped as IMessageTcp);
                                }
                                break;
                            case ProviderType.Websocket:
                                messageTyped = JsonConvert.DeserializeObject<MessageWS>(args.Packet.Data);

                                if (MessageWebsocketEvent != null)
                                {
                                    await MessageWebsocketEvent?.Invoke(sender, messageTyped as IMessageWS);
                                }
                                break;
                            default:
                                break;
                        }

                        if (messageTyped != null && MessageEvent != null)
                        {
                            await MessageEvent?.Invoke(sender, messageTyped);
                        }
                    }
                    catch
                    {
                        if (SystemMessageEvent != null)
                        {
                            await SystemMessageEvent?.Invoke(sender, args.Message);
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        protected virtual async Task OnErrorEvent(object sender, TcpErrorClientEventArgs args)
        {
            if (ErrorEvent != null)
            {
                await ErrorEvent?.Invoke(sender, args);
            }
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