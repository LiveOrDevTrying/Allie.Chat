using Allie.Chat.Lib.DTOs.Commands;

namespace Allie.Chat.Lib.ViewModels.Commands
{
    /// <summary>
    /// A Command Reply ViewModel
    /// </summary>
    public class CommandReplyVM : BaseVM
    {
        /// <summary>
        /// The Command Reply text to send to the outbound streams when a Command is triggered
        /// </summary>>
        public string CommandReplyText { get; set; }
        /// <summary>
        /// The Command the Command Reply is registered
        /// </summary>
        public CommandDTO Command { get; set; }
    }
}
