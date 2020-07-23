using Allie.Chat.Lib.Interfaces;
using System.Threading.Tasks;

namespace Allie.Chat.Websocket.Events
{
    public delegate Task WebsocketMessageEventHandler<T>(object sender, T args) where T : IMessageBase;
    public delegate Task SystemMessageEventHandler(object sender, string message);
}
