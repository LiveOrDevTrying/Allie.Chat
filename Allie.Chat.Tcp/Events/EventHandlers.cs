using Allie.Chat.Lib.Interfaces;

namespace Allie.Chat.Tcp.Events
{
    public delegate void TcpMessageEventHandler<T>(object sender, T args) where T : IMessageBase;
    public delegate void SystemMessageEventHandler(object sender, string message);
}
