using Allie.Chat.Lib.Enums;

namespace Allie.Chat.Lib.ViewModels
{
    /// <summary>
    /// The Provider ViewModel
    /// </summary>
    public class ProviderVM : BaseVM
    {
        /// <summary>
        /// The Provider type
        /// Values: 0 - Twitch, 1 - Discord, 2 - Tcp, 3 - Websocket
        /// </summary>
        public ProviderType ProviderType { get; set; }
        /// <summary>
        /// The Provider type as a string
        /// Values: "Twitch", "Discord", "Tcp", "Websocket"
        /// </summary>
        public string ProviderTypeValue { get; set; }
    }
}
