using Allie.Chat.Lib.Interfaces;
using System.Threading.Tasks;

namespace Allie.Chat.Tcp.Events
{
    public delegate Task TcpMessageEventHandler<T>(object sender, T args) where T : IMessageBase;
    public delegate Task SystemMessageEventHandler(object sender, string message);
}
