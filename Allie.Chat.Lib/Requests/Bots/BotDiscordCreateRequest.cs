using System;
using System.ComponentModel.DataAnnotations;

namespace Allie.Chat.Lib.Requests.Bots
{
    /// <summary>
    /// A Discord Bot create request
    /// </summary>
    public class BotDiscordCreateRequest : BotCreateRequest
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
        /// <summary>
        /// The expiration date and time of the OAuth token
        /// </summary>
        public DateTime ExpireDateTime { get; set; }
        /// <summary>
        /// The Id of the Discord Server that the Bot is registered
        /// </summary>
        public Guid ServerDiscordId { get; set; }
    }
}
