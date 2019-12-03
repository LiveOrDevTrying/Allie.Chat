using Allie.Chat.Lib.DTOs;
using Allie.Chat.Lib.DTOs.Commands;
using System.Collections.Generic;

namespace Allie.Chat.Lib.ViewModels.Commands
{
    /// <summary>
    /// A Command ViewModel
    /// </summary>
    public class CommandVM : BaseVM
    {
        /// <summary>
        /// The text that can be used from any inbound route to trigger a Command Reply
        /// </summary>
        public string CommandText { get; set; }
        /// <summary>
        /// The description of the Command
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// The Command Set the Command is registered
        /// </summary>
        public CommandSetDTO CommandSet { get; set; }
        /// <summary>
        /// The Command Replies registered to the Command
        /// </summary>
        public ICollection<CommandReplyDTO> CommandReplies { get; set; } = new
            List<CommandReplyDTO>();
    }
}
