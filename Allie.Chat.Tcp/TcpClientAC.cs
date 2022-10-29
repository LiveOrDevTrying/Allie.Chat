using Allie.Chat.Lib.Enums;
using Allie.Chat.Lib.Interfaces;
using Allie.Chat.Lib.Structs;
using Allie.Chat.Tcp.Events;
using Newtonsoft.Json;
using PHS.Networking.Enums;
using PHS.Networking.Events;
using System.Threading.Tasks;
using Tcp.NET.Client;
using Tcp.NET.Client.Events.Args;
using Tcp.NET.Client.Models;

namespace Allie.Chat.Tcp
{
    public class TcpClientAC : ITcpClientAC
    {
        protected readonly ITcpNETClient _tcpClient;

        public event NetworkingEventHandler<TcpConnectionClientEventArgs> ConnectionEvent;
        public event TcpMessageEventHandler<IMessageBase> MessageEvent;
        public event TcpMessageEventHandler<IMessageTwitch> MessageTwitchEvent;
        public event TcpMessageEventHandler<IMessageDiscord> MessageDiscordEvent;
        public event TcpMessageEventHandler<IMessageTcp> MessageTcpEvent;
        public event TcpMessageEventHandler<IMessageWS> MessageWebsocketEvent;
        public event NetworkingEventHandler<TcpErrorClientEventArgs> ErrorEvent;
        public event SystemMessageEventHandler SystemMessageEvent;

        public TcpClientAC(string accessToken, string host = "connect.allie.chat", int port = 7625, bool isSSL = true)
        {
            _tcpClient = new TcpNETClient(new ParamsTcpClient(host, port, "\r\n", isSSL, accessToken));
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
                    await DisconnectAsync();
                }

                await _tcpClient.ConnectAsync();

                return true;
            }
            catch
            { }

            return false;
        }
        public virtual async Task DisconnectAsync()
        {
            if (_tcpClient.IsRunning)
            {
                await _tcpClient.DisconnectAsync();
            }
        }
        public virtual async Task<bool> SendAsync(string message)
        {
            return await _tcpClient.SendAsync(message);
        }

        protected virtual void OnConnectionEvent(object sender, TcpConnectionClientEventArgs args)
        {
            ConnectionEvent?.Invoke(sender, args);
        }
        protected virtual void OnMessageEvent(object sender, TcpMessageClientEventArgs args)
        {
            switch (args.MessageEventType)
            {
                case MessageEventType.Sent:
                    break;
                case MessageEventType.Receive:
                    try
                    {
                        var message = JsonConvert.DeserializeObject<MessageBase>(args.Message);
                        IMessageBase messageTyped = null;

                        switch (message.ProviderType)
                        {
                            case ProviderType.Twitch:
                                messageTyped = JsonConvert.DeserializeObject<MessageTwitch>(args.Message);
                                MessageTwitchEvent?.Invoke(sender, messageTyped as IMessageTwitch);
                                break;
                            case ProviderType.Discord:
                                messageTyped = JsonConvert.DeserializeObject<MessageDiscord>(args.Message);
                                MessageDiscordEvent?.Invoke(sender, messageTyped as IMessageDiscord);
                                break;
                            case ProviderType.Tcp:
                                messageTyped = JsonConvert.DeserializeObject<MessageTcp>(args.Message);
                                MessageTcpEvent?.Invoke(sender, messageTyped as IMessageTcp);
                                break;
                            case ProviderType.Websocket:
                                messageTyped = JsonConvert.DeserializeObject<MessageWS>(args.Message);
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
                    break;
                default:
                    break;
            }
        }
        protected virtual void OnErrorEvent(object sender, TcpErrorClientEventArgs args)
        {
            ErrorEvent?.Invoke(sender, args);
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