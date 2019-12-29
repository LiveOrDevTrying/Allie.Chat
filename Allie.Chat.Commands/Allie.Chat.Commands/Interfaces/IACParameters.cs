using Allie.Chat.Commands.Core.Interfaces;
using Allie.Chat.Commands.Enums;

namespace Allie.Chat.Commands.Interfaces
{
    public interface IACParameters : IParameters
    {
        ClientType ClientType { get; set; }
    }
}
