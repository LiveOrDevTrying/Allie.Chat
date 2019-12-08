using Allie.Chat.Lib.DTOs.Servers;
using Allie.Chat.Lib.DTOs.Users;

namespace Allie.Chat.Lib.ViewModels.Bots
{
    /// <summary>
    /// A Discord Bot ViewModel
    /// </summary>
    public class BotDiscordVM : BotVM
    {
        /// <summary>
        /// The Discord Server the Discord Bot is registered to
        /// </summary>
        public ServerDiscordDTO ServerDiscord { get; set; }
    }
}
