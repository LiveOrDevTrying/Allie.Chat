using Allie.Chat.Lib.DTOs.Servers;
using Allie.Chat.Lib.DTOs.Users;

namespace Allie.Chat.Lib.ViewModels.Servers
{
    /// <summary>
    /// A base Server Users ViewModel
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="S"></typeparam>
    public abstract class ServerUsersVM<T, S> where T : ServerDTO where S : UserDTO
    {
        /// <summary>
        /// The Server where the Users are currently online
        /// </summary>
        public T Server { get; set; }
        /// <summary>
        /// The Users online in the Server
        /// </summary>
        public S[] UsersOnline { get; set; }
    }
}
