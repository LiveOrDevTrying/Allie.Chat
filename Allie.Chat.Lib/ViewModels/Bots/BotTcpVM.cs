using Allie.Chat.Lib.Enums;

namespace Allie.Chat.Lib.ViewModels.Bots
{
    /// <summary>
    /// A Tcp Bot ViewModel
    /// </summary>
    public class BotTcpVM : BotVM
    {
        /// <summary>
        /// The Provider type of the Bot User
        /// Values: 0 - Twitch, 1 - Discord, 2 - Tcp, 3 - Websocket
        /// </summary>
        public ProviderType ProviderType { get; set; }
        /// <summary>
        /// The Provider type of the Bot User as a string
        /// Values: "Twitch", "Discord", "Tcp", "Websocket"
        /// </summary>
        public string ProviderTypeValue { get; set; }
        /// <summary>
        /// The token for this Bot to log into the Tcp Server
        /// </summary>
        public string Token { get; set; }
    }
}
