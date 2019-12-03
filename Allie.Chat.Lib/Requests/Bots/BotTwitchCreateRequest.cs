using System.ComponentModel.DataAnnotations;

namespace Allie.Chat.Lib.Requests.Bots
{
    /// <summary>
    /// A Twitch Bot create request
    /// </summary>
    public class BotTwitchCreateRequest : BotCreateRequest
    {
        /// <summary>
        /// The OAuth token provided by Discord
        /// </summary>
        [Required]
        public string OAuthToken { get; set; }
        /// <summary>
        /// The refresh token provided by Discord
        /// </summary>
        [Required]
        public string RefreshToken { get; set; }
    }
}
