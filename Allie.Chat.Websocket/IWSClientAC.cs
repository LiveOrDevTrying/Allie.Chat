using System;
using System.Threading.Tasks;
using Allie.Chat.Lib.Interfaces;
using Allie.Chat.Websocket.Events;
using PHS.Networking.Events;
using WebsocketsSimple.Client.Events.Args;

namespace Allie.Chat.Websocket
{
    public interface IWSClientAC : IDisposable
    {
        bool IsRunning { get; }

        event NetworkingEventHandler<WSConnectionClientEventArgs> ConnectionEvent;
        event NetworkingEventHandler<WSErrorClientEventArgs> ErrorEvent;
        event WebsocketMessageEventHandler<IMessageDiscord> MessageDiscordEvent;
        event WebsocketMessageEventHandler<IMessageBase> MessageEvent;
        event WebsocketMessageEventHandler<IMessageTcp> MessageTcpEvent;
        event WebsocketMessageEventHandler<IMessageTwitch> MessageTwitchEvent;
        event WebsocketMessageEventHandler<IMessageWS> MessageWebsocketEvent;
        event SystemMessageEventHandler SystemMessageEvent;

        Task<bool> ConnectAsync();
        Task<bool> DisconnectAsync();
        Task<bool> SendAsync(string message);
    }
}