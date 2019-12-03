using Allie.Chat.Lib.DTOs.Users;

namespace Allie.Chat.Lib.Interfaces
{
    /// <summary>
    /// A Websocket Message interface
    /// </summary>
    public interface IMessageWS : IMessageGeneric<UserWSDTO>
    {
    }
}