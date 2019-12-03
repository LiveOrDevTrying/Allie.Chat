using Allie.Chat.Lib.DTOs.Users;
using System.ComponentModel.DataAnnotations;

namespace Allie.Chat.Lib.ViewModels.Servers
{
    /// <summary>
    /// A Twitch Server ViewModel
    /// </summary>
    public class ServerTwitchVM : ServerVM
    {
        /// <summary>
        /// The Server's Twitch display name
        /// </summary>
        public string TwitchDisplayName { get; set; }
        /// <summary>
        /// The Twitch User that owns this Server
        /// </summary>
        public UserTwitchDTO UserTwitch { get; set; }
    }
}