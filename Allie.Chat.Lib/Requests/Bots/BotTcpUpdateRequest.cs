namespace Allie.Chat.Lib.Requests.Bots
{
    /// <summary>
    /// A Tcp Bot update request
    /// </summary>
    public class BotTcpUpdateRequest : BotUpdateRequest
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
    }
}
