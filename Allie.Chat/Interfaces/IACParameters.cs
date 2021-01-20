using Allie.Chat.Enums;

namespace Allie.Chat.Interfaces
{
    public interface IACParameters : IParameters
    {
        ClientType ClientType { get; set; }
    }
}
