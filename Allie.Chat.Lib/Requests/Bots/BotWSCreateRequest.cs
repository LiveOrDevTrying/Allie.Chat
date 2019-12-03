using System.ComponentModel.DataAnnotations;

namespace Allie.Chat.Lib.Requests.Bots
{
    /// <summary>
    /// A Websocket Bot create request
    /// </summary>
    public class BotWSCreateRequest : BaseRequest
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
