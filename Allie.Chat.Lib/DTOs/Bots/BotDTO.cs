using Allie.Chat.Lib.Enums;

namespace Allie.Chat.Lib.DTOs.Bots
{
    /// <summary>
    /// A base Bot data-transfer object
    /// </summary>
    public class BotDTO : BaseDTO
    {
        /// <summary>
        /// The username of the Bot User
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// The display name of the Bot User
        /// </summary>
        public string DisplayName { get; set; }
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
