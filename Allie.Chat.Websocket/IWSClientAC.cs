using System;
using System.Threading.Tasks;
using Allie.Chat.Lib.Interfaces;
using PHS.Core.Events;
using WebsocketsSimple.Core.Events.Args;

namespace Allie.Chat.Websocket
{
    public interface IWSClientAC : IDisposable
    {
        bool IsRunning { get; }

        event NetworkingEventHandler<WSConnectionEventArgs> ConnectionEvent;
        event NetworkingEventHandler<WSErrorEventArgs> ErrorEvent;
        event WebsocketMessageEventHandler<IMessageDiscord> MessageDiscordEvent;
        event WebsocketMessageEventHandler<IMessageBase> MessageEvent;
        event WebsocketMessageEventHandler<IMessageTcp> MessageTcpEvent;
        event WebsocketMessageEventHandler<IMessageTwitch> MessageTwitchEvent;
        event WebsocketMessageEventHandler<IMessageWS> MessageWebsocketEvent;

        Task<bool> SendAsync(string message);
    }
}