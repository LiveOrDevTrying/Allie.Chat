namespace Allie.Chat.Lib.Requests.Users
{
    /// <summary>
    /// A Twitch User create request
    /// </summary>
    public class UserTwitchCreateRequest : UserCreateRequest
    {
        /// <summary>
        /// The Id assigned by Twitch
        /// </summary>
        public string TwitchId { get; set; }
    }
}
