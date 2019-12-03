using System;
using System.ComponentModel.DataAnnotations;

namespace Allie.Chat.Lib.Requests.Bots
{
    /// <summary>
    /// A Discord Bot update request
    /// </summary>
    public class BotDiscordUpdateRequest : BaseRequest
    {
        /// <summary>
        /// The Id of the existing Discord Bot
        /// </summary>
        [Required]
        public Guid Id { get; set; }
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
        /// <summary>
        /// The expiration date and time of the OAuth token
        /// </summary>
        public DateTime ExpireDateTime { get; set; }
    }
}
