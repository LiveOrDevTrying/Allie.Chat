using Allie.Chat.Commands.Service.Events.Args;
using Allie.Chat.Commands.Service.Services;
using Allie.Chat.Lib.Interfaces;

namespace Allie.Chat.Commands.Service
{
    public interface ICommandsService : IBasePollingService
    {
        CommandEventArgs[] ProcessMessage(IMessageBase message);
        void UpdateWebAPIToken(string webAPIToken);
    }
}