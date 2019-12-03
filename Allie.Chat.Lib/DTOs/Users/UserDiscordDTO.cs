namespace Allie.Chat.Lib.DTOs.Users
{
    /// <summary>
    /// A Discord User data-transfer object
    /// </summary>
    public class UserDiscordDTO : UserDTO
    {
        /// <summary>
        /// The Id of the User from Discord
        /// </summary>
        public string DiscordId { get; set; }
    }
}
