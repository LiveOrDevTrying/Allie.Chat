using System.ComponentModel.DataAnnotations;

namespace Allie.Chat.Lib.Requests.Users
{
    /// <summary>
    /// A Discord User create request
    /// </summary>
    public class UserDiscordCreateRequest : UserCreateRequest
    {
        /// <summary>
        /// The Id of the User from Discord
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string DiscordId { get; set; }
    }
}
