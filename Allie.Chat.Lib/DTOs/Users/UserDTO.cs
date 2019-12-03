using Allie.Chat.Lib.Enums;

namespace Allie.Chat.Lib.DTOs.Users
{
    /// <summary>
    /// A base User data-transfer object
    /// </summary>
    public class UserDTO : BaseDTO
    {
        /// <summary>
        /// The User's username
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// The User's avatar url
        /// </summary>
        public string AvatarUrl { get; set; }
        /// <summary>
        /// The User's display name
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
