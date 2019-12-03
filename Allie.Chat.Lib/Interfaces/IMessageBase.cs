using Allie.Chat.Lib.DTOs.Paths;
using Allie.Chat.Lib.DTOs.Routes;
using Allie.Chat.Lib.DTOs.Servers;
using Allie.Chat.Lib.DTOs.Servers.Channels;
using Allie.Chat.Lib.DTOs.Streams;
using Allie.Chat.Lib.DTOs.Users;
using Allie.Chat.Lib.Enums;

namespace Allie.Chat.Lib.Interfaces
{
    /// <summary>
    /// A base Message interface
    /// </summary>
    public interface IMessageBase
    {
        /// <summary>
        /// The message text
        /// </summary>
        string Message { get; set; }
        /// <summary>
        /// The Provider type of the Bot User
        /// Values: 0 - Twitch, 1 - Discord, 2 - Tcp, 3 - Websocket
        /// </summary>
        ProviderType ProviderType { get; set; }
        /// <summary>
        /// The Provider type of the Bot User as a string
        /// Values: "Twitch", "Discord", "Tcp", "Websocket"
        /// </summary>
        string ProviderTypeValue { get; set; }
        /// <summary>
        /// The Stream that received the message
        /// </summary>
        StreamDTO Stream { get; set; }
        /// <summary>
        /// The inbound Route that received the message
        /// </summary>
        RouteDTO RouteSource { get; set; }
        /// <summary>
        /// The outbound Route where the message was sent
        /// </summary>
        RouteDTO Route { get; set; }
        /// <summary>
        /// The Path that received the message
        /// </summary>
        PathDTO PathSource { get; set; }
        /// <summary>
        /// The Path where the message was sent
        /// </summary>
        PathDTO Path { get; set; }
        /// <summary>
        /// The Server that received the message
        /// </summary>
        ServerDTO ServerSource { get; set; }
        /// <summary>
        /// The Channel that received the message
        /// </summary>
        ServerChannelDTO ChannelSource { get; set; }
        /// <summary>
        /// The User that received the message
        /// </summary>
        UserDTO UserSource { get; set; }
        /// <summary>
        /// The Chat Color of the Message
        /// </summary>
        string ChatColor { get; set; }
    }
}