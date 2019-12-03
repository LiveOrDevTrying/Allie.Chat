using System;

namespace Allie.Chat.Lib.Requests.Users
{
    /// <summary>
    /// A base User create request
    /// </summary>
    public abstract class UserCreateRequest : BaseRequest
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
        /// The Id of the User's provider
        /// </summary>
        public Guid ProviderId { get; set; }
    }
}
