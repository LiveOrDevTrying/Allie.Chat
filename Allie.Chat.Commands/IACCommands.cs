using Allie.Chat.Commands.Service.Events;
using Allie.Chat.Commands.Service.Events.Args;
using PHS.Networking.Events.Args;
using System;
using System.Threading.Tasks;

namespace Allie.Chat.Commands
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
        event ClientEventHandler<ConnectionEventArgs> ConnectionEvent;
        event ClientEventHandler<ErrorEventArgs> ErrorEvent;
    }
}