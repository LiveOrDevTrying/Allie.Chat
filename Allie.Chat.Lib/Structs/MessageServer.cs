using Allie.Chat.Lib.DTOs.Paths;
using Allie.Chat.Lib.DTOs.Routes;
using Allie.Chat.Lib.DTOs.Servers;
using Allie.Chat.Lib.DTOs.Servers.Channels;
using Allie.Chat.Lib.DTOs.Streams;
using Allie.Chat.Lib.DTOs.Users;
using Allie.Chat.Lib.Enums;
using Allie.Chat.Lib.Interfaces;

namespace Allie.Chat.Lib.Structs
{
    /// <summary>
    /// A base Server Message struct
    /// </summary>
    /// <typeparam name="T">A User data-transfer object</typeparam>
    /// <typeparam name="S">A Server data-transfer object</typeparam>
    public struct MessageServer<T, S> : IMessageServer<T, S> 
        where T : UserDTO
        where S : ServerDTO
    {
        /// <summary>
        /// The message text
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// The Provider type of the Bot User
        /// Values: 0 - Twitch, 1 - Discord, 2 - Tcp, 3 - Websocket
        /// </summary>
        public ProviderType ProviderType { get; set; }
        /// <summary>
        /// The Provider type of the Bot User as a string
        /// Values: "Twitch", "Discord", "Tcp", "Websocket"
        /// </summary>
        public string ProviderTypeValue { get; set; }
        /// <summary>
        /// The Chat Color of the Message
        /// </summary>
        public string ChatColor { get; set; }
        /// <summary>
        /// The User that sent the message
        /// </summary>
        public T User { get; set; }
        /// <summary>
        /// The Bot User that received the message
        /// </summary>
        public UserDTO UserSource { get; set; }
        /// <summary>
        /// The Stream that received the message
        /// </summary>
        public StreamDTO Stream { get; set; }
        /// <summary>
        /// The outbound Route where the message was sent
        /// </summary>
        public RouteDTO Route { get; set; }
        /// <summary>
        /// The Path where the message was sent
        /// </summary>
        public PathDTO Path { get; set; }
        /// <summary>
        /// The Server where the message was sent
        /// </summary>
        public S Server { get; set; }
        /// <summary>
        /// The inbound Route that received the message
        /// </summary>
        public RouteDTO RouteSource { get; set; }
        /// <summary>
        /// The Path that received the message
        /// </summary>
        public PathDTO PathSource { get; set; }
        /// <summary>
        /// The Server that received the message
        /// </summary>
        public ServerDTO ServerSource { get; set; }
        /// <summary>
        /// The Channel that received the message
        /// </summary>
        public ServerChannelDTO ChannelSource { get; set; }
    }
}
