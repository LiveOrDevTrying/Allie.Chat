using Allie.Chat.Lib.DTOs.Users;

namespace Allie.Chat.Lib.ViewModels.Bots
{
    /// <summary>
    /// A Twitch Bot ViewModel
    /// </summary>
    public class BotTwitchVM : BotVM
    {
        /// <summary>
        /// The Twitch User the Twitch Bot is registered to
        /// </summary>
        public UserDTO User { get; set; }
    }
}
