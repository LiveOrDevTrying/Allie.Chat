using Allie.Chat.Commands.Core.Events;
using Allie.Chat.Commands.Core.Events.Args;
using Allie.Chat.Commands.Core.Services;

namespace Allie.Chat.Commands.Core
{
    public interface ICommandsService : IBasePollingService
    {
        event CommandEventHandler<CommandDiscordEventArgs> CommandDiscordEvent;
        event CommandEventHandler<CommandEventArgs> CommandEvent;
        event CommandEventHandler<CommandTcpEventArgs> CommandTcpEvent;
        event CommandEventHandler<CommandWSEventArgs> CommandWebsocketEvent;
        event CommandEventHandler<CommandTwitchEventArgs> CommndTwitchEvent;

        void UpdateWebAPIToken(string webAPIToken);
    }
}