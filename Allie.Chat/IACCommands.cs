using Allie.Chat.Events;
using Allie.Chat.Events.Args;
using PHS.Networking.Events.Args;
using System;
using System.Threading.Tasks;

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
        event ClientEventHandler<ConnectionEventArgs> ConnectionEvent;
        event ClientEventHandler<ErrorEventArgs> ErrorEvent;
    }
}