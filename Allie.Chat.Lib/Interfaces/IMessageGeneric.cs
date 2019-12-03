using Allie.Chat.Lib.DTOs.Users;

namespace Allie.Chat.Lib.Interfaces
{
    /// <summary>
    /// A base Generic Message interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMessageGeneric<T> : IMessageBase where T : UserDTO
    {
        /// <summary>
        /// The Bot User that received the message
        /// </summary>
        T User { get; set; }
    }
}