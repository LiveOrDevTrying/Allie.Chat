using Allie.Chat.Events.Args;
using PHS.Core.Events.Args;
using System.Threading.Tasks;

namespace Allie.Chat.Events
{
    public delegate Task CommandEventHandler<T>(object sender, T args) where T : BaseEventArgs;
    public delegate Task ClientEventHandler<T>(object sender, T args) where T : BaseArgs;
}
