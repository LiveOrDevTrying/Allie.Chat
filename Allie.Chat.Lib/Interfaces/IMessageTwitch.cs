using Allie.Chat.Lib.DTOs.Servers;
using Allie.Chat.Lib.DTOs.Users;

namespace Allie.Chat.Lib.Interfaces
{
    /// <summary>
    /// A Twitch Message interface
    /// </summary>
    public interface IMessageTwitch : IMessageServer<UserTwitchDTO, ServerTwitchDTO>
    {
    }
}