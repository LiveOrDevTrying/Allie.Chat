﻿using Allie.Chat.Commands.Core.Events;
using Allie.Chat.Commands.Core.Events.Args;
using Allie.Chat.Commands.Core.Services;
using System.Threading.Tasks;

using PHS.Core;
using PHS.Core.Events.Args.NetworkEventArgs;

namespace Allie.Chat.Commands.Core
{
    public interface ICommandsService : IBasePollingService
    {
        Task SendMessageAsync(string message);

        event ClientEventHandler<ConnectionEventArgs> ConnectionEvent;
        event ClientEventHandler<ErrorEventArgs> ErrorEvent;
        event CommandEventHandler<CommandDiscordEventArgs> CommandDiscordEvent;
        event CommandEventHandler<CommandEventArgs> CommandEvent;
        event CommandEventHandler<CommandTcpEventArgs> CommandTcpEvent;
        event CommandEventHandler<CommandWSEventArgs> CommandWebsocketEvent;
        event CommandEventHandler<CommandTwitchEventArgs> CommandTwitchEvent;
    }
}