using Allie.Chat.Lib.DTOs.Commands;
using Allie.Chat.Lib.Interfaces;

namespace Allie.Chat.Commands.Core.Events.Args
{
    public class CommandEventArgs : BaseEventArgs
    {
        public IMessageBase Message { get; set; }
        public CommandSetDTO CommandSet { get; set; }
        public CommandDTO Command { get; set; }
    }

    public abstract class CommandEventArgs<T> : CommandEventArgs
        where T : IMessageBase
    {
        public new T Message { get; set; }
    }
}
