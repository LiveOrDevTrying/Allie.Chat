namespace Allie.Chat.Lib.DTOs.Users
{
    /// <summary>
    /// A Twitch User data-transfer object
    /// </summary>
    public class UserTwitchDTO : UserDTO
    {
        /// <summary>
        /// The Id assigned by Twitch
        /// </summary>
        public string TwitchId { get; set; }
    }
}
