using Allie.Chat.Commands.Core.Events;
using Allie.Chat.Commands.Core.Events.Args;
using PHS.Core.Events.Args.NetworkEventArgs;

namespace Allie.Chat.Commands
{
    public interface IACCommands
    {
        event CommandEventHandler<CommandDiscordEventArgs> CommandDiscordEvent;
        event CommandEventHandler<CommandEventArgs> CommandEvent;
        event CommandEventHandler<CommandTcpEventArgs> CommandTcpEvent;
        event CommandEventHandler<CommandTwitchEventArgs> CommandTwitchEvent;
        event CommandEventHandler<CommandWSEventArgs> CommandWebsocketEvent;
        event ClientEventHandler<ConnectionEventArgs> ConnectionEvent;
        event ClientEventHandler<ErrorEventArgs> ErrorEvent;

        void Dispose();
    }
}