using Allie.Chat.Lib.Enums;
using Allie.Chat.Lib.Interfaces;
using Allie.Chat.Lib.Structs;
using Newtonsoft.Json;
using PHS.Core.Enums;
using PHS.Core.Events;
using PHS.Core.Models;
using System;
using System.Threading.Tasks;
using WebsocketsSimple.Client;
using WebsocketsSimple.Core.Events.Args;

namespace Allie.Chat.Websocket
{
    public delegate void WebsocketMessageEventHandler<T>(object sender, T args) where T : IMessageBase;

    public class WSClient : IWSClient
    {
        private readonly string _connectUri = "https://connect.allie.chat";
        private readonly int _connectPort = 7420;
        protected readonly WebsocketClient _websocketClient;

        public event NetworkingEventHandler<WSConnectionEventArgs> ConnectionEvent;
        public event WebsocketMessageEventHandler<IMessageBase> MessageEvent;
        public event WebsocketMessageEventHandler<IMessageTwitch> MessageTwitchEvent;
        public event WebsocketMessageEventHandler<IMessageDiscord> MessageDiscordEvent;
        public event WebsocketMessageEventHandler<IMessageTcp> MessageTcpEvent;
        public event WebsocketMessageEventHandler<IMessageWS> MessageWebsocketEvent;
        public event NetworkingEventHandler<WSErrorEventArgs> ErrorEvent;

        public WSClient(string accessToken)
        {
            _websocketClient = new WebsocketClient();
            _websocketClient.ConnectionEvent += OnConnectionEvent;
            _websocketClient.MessageEvent += OnMessageEvent;
            _websocketClient.ErrorEvent += OnErrorEvent;
            _websocketClient.Start(_connectUri, _connectPort, accessToken, true);
        }
        public WSClient(string url, int port, string accessToken)
        {
            _connectUri = url;
            _connectPort = port;

            _websocketClient = new WebsocketClient();
            _websocketClient.ConnectionEvent += OnConnectionEvent;
            _websocketClient.MessageEvent += OnMessageEvent;
            _websocketClient.ErrorEvent += OnErrorEvent;
            _websocketClient.Start(_connectUri, _connectPort, accessToken, url.ToLower().Contains("https"));
        }

        public virtual async Task<bool> SendAsync(string message)
        {
            return await _websocketClient.Send(new PacketDTO
            {
                Action = (int)ActionType.SendToServer,
                Data = message,
                Timestamp = DateTime.UtcNow
            });
        }

        protected virtual Task OnConnectionEvent(object sender, WSConnectionEventArgs args)
        {
            ConnectionEvent(sender, args);
            return Task.CompletedTask;
        }
        protected virtual async Task OnMessageEvent(object sender, WSMessageEventArgs args)
        {
            switch (args.MessageEventType)
            {
                case MessageEventType.Sent:
                    break;
                case MessageEventType.Receive:
                    if (args.Message.Trim().ToLower() == "ping")
                    {
                        await _websocketClient.Send("pong");
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
        }
        protected virtual Task OnErrorEvent(object sender, WSErrorEventArgs args)
        {
            ErrorEvent(sender, args);
            return Task.CompletedTask;
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
