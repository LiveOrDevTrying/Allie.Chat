using Allie.Chat.Lib.DTOs.Servers;

namespace Allie.Chat.Lib.ViewModels.Users
{
    /// <summary>
    /// A Twitch User ViewModel
    /// </summary>
    public class UserTwitchVM : UserVM
    {
        /// <summary>
        /// The Id assigned by Twitch
        /// </summary>
        public string TwitchId { get; set; }
        /// <summary>
        /// The User's Twitch Server
        /// </summary>
        public ServerTwitchDTO ServerTwitch { get; set; }
    }
}
