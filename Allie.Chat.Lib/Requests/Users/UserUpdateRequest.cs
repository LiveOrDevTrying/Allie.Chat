namespace Allie.Chat.Lib.Requests.Users
{
    /// <summary>
    /// A base User update request
    /// </summary>
    public abstract class UserUpdateRequest : UpdateRequest
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
    }
}
