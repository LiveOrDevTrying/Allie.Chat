namespace Allie.Chat.Lib.DTOs.Commands
{
    /// <summary>
    /// A Command Reply data-transfer object
    /// </summary>
    public class CommandReplyDTO : BaseDTO
    {
        /// <summary>
        /// The Command Reply text to send to the outbound streams when a Command is triggered
        /// </summary>
        public string CommandReplyText { get; set; }
    }
}
