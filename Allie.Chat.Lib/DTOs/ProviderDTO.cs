using Allie.Chat.Lib.Enums;

namespace Allie.Chat.Lib.DTOs
{
    /// <summary>
    /// A Provider data-transfer object
    /// </summary>
    public class ProviderDTO : BaseDTO
    {
        /// <summary>
        /// The Provider Type
        /// Values: 0 - Twitch, 1 - Discord, 2 - Tcp, 3 - Websocket
        /// </summary>
        public ProviderType ProviderType { get; set; }
        /// <summary>
        /// The Provider type as a string
        /// Values: "Twitch", "Discord", "Tcp", "Websocket"
        /// </summary>
        public string ProviderTypeValue { get; set; }
    }
}
