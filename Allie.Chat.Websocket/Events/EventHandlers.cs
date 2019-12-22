using Allie.Chat.Lib.Interfaces;

namespace Allie.Chat.Websocket.Events
{
    public delegate void WebsocketMessageEventHandler<T>(object sender, T args) where T : IMessageBase;
    public delegate void SystemMessageEventHandler(object sender, string message);
}
