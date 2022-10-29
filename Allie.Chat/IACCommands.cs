using Allie.Chat.Events;
using Allie.Chat.Events.Args;
using PHS.Networking.Events.Args;
using PHS.Networking.Models;
using System;
using System.Threading.Tasks;
using Tcp.NET.Core.Models;
using WebsocketsSimple.Core.Models;

namespace Allie.Chat
{
    public interface IACCommands : IDisposable
    {
        Task StartAsync();
        void Update(int updateIntervalMS);

        event CommandEventHandler<CommandDiscordEventArgs> CommandDiscordEvent;
        event CommandEventHandler<CommandEventArgs> CommandEvent;
        event CommandEventHandler<CommandTcpEventArgs> CommandTcpEvent;
        event CommandEventHandler<CommandTwitchEventArgs> CommandTwitchEvent;
        event CommandEventHandler<CommandWSEventArgs> CommandWebsocketEvent;
        event ClientEventHandler<ConnectionEventArgs<ConnectionWS>> ConnectionWSEvent;
        event ClientEventHandler<ConnectionEventArgs<ConnectionTcp>> ConnectionTcpEvent;
        event ClientEventHandler<ErrorEventArgs<ConnectionWS>> ErrorWSEvent;
        event ClientEventHandler<ErrorEventArgs<ConnectionTcp>> ErrorTcpEvent;
    }
}