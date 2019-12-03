using Allie.Chat.Lib.DTOs;
using Allie.Chat.Lib.DTOs.Servers;
using Allie.Chat.Lib.DTOs.Users;

namespace Allie.Chat.Lib.Interfaces
{
    /// <summary>
    /// A base Server Message interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="S"></typeparam>
    public interface IMessageServer<T, S> : IMessageGeneric<T>
        where T  : UserDTO
        where S : ServerDTO
    {
        /// <summary>
        /// The Server where the Message was sent
        /// </summary>
        S Server { get; set; }
    }
}