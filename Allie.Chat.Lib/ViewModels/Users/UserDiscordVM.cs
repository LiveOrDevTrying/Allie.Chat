namespace Allie.Chat.Lib.ViewModels.Users
{
    /// <summary>
    /// A Discord User ViewModel
    /// </summary>
    public class UserDiscordVM : UserVM
    {
        /// <summary>
        /// The Id of the User from Discord
        /// </summary>
        public string DiscordId { get; set; }
    }
}
