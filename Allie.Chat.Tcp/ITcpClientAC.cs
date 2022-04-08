using System;
using System.Threading.Tasks;
using Allie.Chat.Lib.Interfaces;
using Allie.Chat.Tcp.Events;
using PHS.Networking.Events;
using Tcp.NET.Client.Events.Args;

namespace Allie.Chat.Tcp
{
    public interface ITcpClientAC : IDisposable
    {
        bool IsRunning { get; }

        event NetworkingEventHandler<TcpConnectionClientEventArgs> ConnectionEvent;
        event NetworkingEventHandler<TcpErrorClientEventArgs> ErrorEvent;
        event TcpMessageEventHandler<IMessageDiscord> MessageDiscordEvent;
        event TcpMessageEventHandler<IMessageBase> MessageEvent;
        event TcpMessageEventHandler<IMessageTcp> MessageTcpEvent;
        event TcpMessageEventHandler<IMessageTwitch> MessageTwitchEvent;
        event TcpMessageEventHandler<IMessageWS> MessageWebsocketEvent;
        event SystemMessageEventHandler SystemMessageEvent;

        Task<bool> ConnectAsync();
        bool Disconnect();

        Task<bool> SendAsync(string message);
    }
}