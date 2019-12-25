using System.Threading.Tasks;
using Allie.Chat.Lib.Interfaces;
using Allie.Chat.Tcp.Events;
using PHS.Core.Events;
using Tcp.NET.Core.SSL.Events.Args;

namespace Allie.Chat.Tcp
{
    public interface ITcpClientSSLAC
    {
        bool IsRunning { get; }

        event NetworkingEventHandler<TcpSSLConnectionEventArgs> ConnectionEvent;
        event NetworkingEventHandler<TcpSSLErrorEventArgs> ErrorEvent;
        event TcpMessageEventHandler<IMessageDiscord> MessageDiscordEvent;
        event TcpMessageEventHandler<IMessageBase> MessageEvent;
        event TcpMessageEventHandler<IMessageTcp> MessageTcpEvent;
        event TcpMessageEventHandler<IMessageTwitch> MessageTwitchEvent;
        event TcpMessageEventHandler<IMessageWS> MessageWebsocketEvent;
        event SystemMessageEventHandler SystemMessageEvent;

        Task<bool> ConnectAsync();
        bool Disconnect();

        void Dispose();
        Task<bool> SendAsync(string message);
    }
}