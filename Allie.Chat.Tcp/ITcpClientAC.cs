using System.Threading.Tasks;
using Allie.Chat.Lib.Interfaces;
using PHS.Core.Events;
using Tcp.NET.Core.Events.Args;

namespace Allie.Chat.Tcp
{
    public interface ITcpClientAC
    {
        bool IsRunning { get; }

        event NetworkingEventHandler<TcpConnectionEventArgs> ConnectionEvent;
        event NetworkingEventHandler<TcpErrorEventArgs> ErrorEvent;
        event TcpMessageEventHandler<IMessageDiscord> MessageDiscordEvent;
        event TcpMessageEventHandler<IMessageBase> MessageEvent;
        event TcpMessageEventHandler<IMessageTcp> MessageTcpEvent;
        event TcpMessageEventHandler<IMessageTwitch> MessageTwitchEvent;
        event TcpMessageEventHandler<IMessageWS> MessageWebsocketEvent;

        void Dispose();
        Task<bool> SendAsync(string message);
    }
}