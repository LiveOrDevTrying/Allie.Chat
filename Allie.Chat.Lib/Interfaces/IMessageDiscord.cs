using Allie.Chat.Lib.DTOs.Servers;
using Allie.Chat.Lib.DTOs.Servers.Channels;
using Allie.Chat.Lib.DTOs.Users;

namespace Allie.Chat.Lib.Interfaces
{
    /// <summary>
    /// A Discord Message interface
    /// </summary>
    public interface IMessageDiscord : IMessageServer<UserDiscordDTO, ServerDiscordDTO>
    {
        /// <summary>
        /// The Discord Channel where the Message was sent
        /// </summary>
        ServerChannelDiscordDTO Channel { get; set; }
    }
}