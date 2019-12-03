using System.ComponentModel.DataAnnotations;

namespace Allie.Chat.Lib.Requests.Bots
{
    /// <summary>
    /// A Websocket Bot update request
    /// </summary>
    public class BotWSUpdateRequest : BotUpdateRequest
    {
        /// <summary>
        /// The User's username
        /// </summary>
        [Required]
        public string Username { get; set; }
        /// <summary>
        /// The User's avatar url
        /// </summary>
        public string AvatarUrl { get; set; }
        /// <summary>
        /// The User's display name
        /// </summary>
        public string DisplayName { get; set; }
    }
}
