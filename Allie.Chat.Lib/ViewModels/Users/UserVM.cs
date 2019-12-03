using Allie.Chat.Lib.DTOs;
using Allie.Chat.Lib.DTOs.Servers;

namespace Allie.Chat.Lib.ViewModels.Users
{
    /// <summary>
    /// A base User ViewModel
    /// </summary>
    public class UserVM : BaseVM
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

        /// <summary>
        /// The User's Provider
        /// </summary>
        public ProviderDTO Provider { get; set; }
    }
}
