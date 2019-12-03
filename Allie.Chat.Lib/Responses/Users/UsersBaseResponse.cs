using Allie.Chat.Lib.DTOs;
using Allie.Chat.Lib.DTOs.Users;

namespace Allie.Chat.Lib.Responses.Users
{
    /// <summary>
    /// A Users base response
    /// </summary>
    public abstract class UsersBaseResponse<T> where T : UserDTO
    {
        /// <summary>
        /// Thie Users requested
        /// </summary>
        public T[] Users { get; set; }
        /// <summary>
        /// The User Ids of users that were not located
        /// </summary>
        public string[] RejectedIds { get; set; }
    }
}
