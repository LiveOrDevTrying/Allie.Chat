using Allie.Chat.Lib.Enums;
using Allie.Chat.Lib.Interfaces;
using Allie.Chat.Lib.Structs;
using Allie.Chat.Websocket.Events;
using Newtonsoft.Json;
using PHS.Networking.Enums;
using PHS.Networking.Events;
using PHS.Networking.Models;
using System;
using System.Threading.Tasks;
using WebsocketsSimple.Client;
using WebsocketsSimple.Client.Events.Args;
using WebsocketsSimple.Client.Models;

namespace Allie.Chat.Websocket
{
    public class WSClientAC : IWSClientAC
    {
        protected readonly IWebsocketClient _websocketClient;

        public event NetworkingEventHandler<WSConnectionClientEventArgs> ConnectionEvent;
        public event WebsocketMessageEventHandler<IMessageBase> MessageEvent;
        public event WebsocketMessageEventHandler<IMessageTwitch> MessageTwitchEvent;
        public event WebsocketMessageEventHandler<IMessageDiscord> MessageDiscordEvent;
        public event WebsocketMessageEventHandler<IMessageTcp> MessageTcpEvent;
        public event WebsocketMessageEventHandler<IMessageWS> MessageWebsocketEvent;
        public event NetworkingEventHandler<WSErrorClientEventArgs> ErrorEvent;
        public event SystemMessageEventHandler SystemMessageEvent;

        public WSClientAC(string accessToken, string host = "connect.allie.chat", int port = 7615, bool isWSS = true)
        {
            _websocketClient = new WebsocketClient(new ParamsWSClient
            {
                IsWebsocketSecured = isWSS,
                Port = port,
                Host = host
            }, accessToken);
            _websocketClient.ConnectionEvent += OnConnectionEvent;
            _websocketClient.MessageEvent += OnMessageEvent;
            _websocketClient.ErrorEvent += OnErrorEvent;
        }

        public virtual async Task<bool> ConnectAsync()
        {
            return await _websocketClient.ConnectAsync();
        }
        public virtual async Task<bool> DisconnectAsync()
        {
            return await _websocketClient.DisconnectAsync();
        }
        public virtual async Task<bool> SendAsync(string message)
        {
            return await _websocketClient.SendToServerAsync(message);
        }

        protected virtual void OnConnectionEvent(object sender, WSConnectionClientEventArgs args)
        {
            ConnectionEvent?.Invoke(sender, args);
        }
        protected virtual void OnMessageEvent(object sender, WSMessageClientEventArgs args)
        {
            switch (args.MessageEventType)
            {
                case MessageEventType.Sent:
                    break;
                case MessageEventType.Receive:
                    if (args.Message.Trim().ToLower() == "ping")
                    {
                        Task.Run(async () => await _websocketClient.SendToServerAsync("pong"));
                    }
                    else
                    {
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
                    }
                    break;
                default:
                    break;
            }
        }
        protected virtual void OnErrorEvent(object sender, WSErrorClientEventArgs args)
        {
            ErrorEvent?.Invoke(sender, args);
        }

        public virtual void Dispose()
        {
            if (_websocketClient != null)
            {
                _websocketClient.ConnectionEvent -= OnConnectionEvent;
                _websocketClient.MessageEvent -= OnMessageEvent;
                _websocketClient.ErrorEvent -= OnErrorEvent;
                _websocketClient.Dispose();
            }
        }

        public bool IsRunning
        {
            get
            {
                return _websocketClient != null && _websocketClient.IsRunning;
            }
        }
    }
}
