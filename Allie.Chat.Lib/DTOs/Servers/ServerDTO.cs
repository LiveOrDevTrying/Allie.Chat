using Allie.Chat.Lib.Enums;

namespace Allie.Chat.Lib.DTOs.Servers
{
    /// <summary>
    /// A Server data-transfer object
    /// </summary>
    public class ServerDTO : BaseDTO
    {
        /// <summary>
        /// The name of the Server
        /// </summary>
        public string Name { get; set; }
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
    }
}
