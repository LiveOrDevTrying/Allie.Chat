using Allie.Chat.Lib.DTOs.Streams;
using Allie.Chat.Lib.DTOs.Users;

namespace Allie.Chat.Lib.ViewModels.Streams
{
    /// <summary>
    /// A StreamUsers ViewModel
    /// </summary>
    public class StreamUsersVM
    {
        /// <summary>
        /// The Stream that contains the Users
        /// </summary>
        public StreamDTO Stream { get; set; }
        /// <summary>
        /// The Users online in the Stream
        /// </summary>
        public UserDTO[] UsersOnline { get; set; }
    }
}
