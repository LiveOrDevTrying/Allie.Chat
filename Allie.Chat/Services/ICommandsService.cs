using Allie.Chat.Events.Args;
using Allie.Chat.Services;
using Allie.Chat.Lib.Interfaces;

namespace Allie.Chat.Services
{
    public interface ICommandsService : IBasePollingService
    {
        CommandEventArgs[] ProcessMessage(IMessageBase message);
        void UpdateWebAPIToken(string webAPIToken);
    }
}