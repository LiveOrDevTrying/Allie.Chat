using Allie.Chat.Lib.DTOs;
using Allie.Chat.Lib.Enums;

namespace Allie.Chat.Lib.ViewModels.Servers
{
    /// <summary>
    /// A base Server ViewModel
    /// </summary>
    public abstract class ServerVM : BaseVM
    {
        /// <summary>
        /// The name of the Server
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Is the server currently online?
        /// </summary>
        public bool IsOnline { get; set; }
        /// <summary>
        /// The Provider type of the Bot User
        /// </summary>
        public ProviderDTO Provider { get; set; }
    }
}
