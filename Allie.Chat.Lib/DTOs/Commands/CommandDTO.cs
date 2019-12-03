namespace Allie.Chat.Lib.DTOs.Commands
{
    /// <summary>
    /// A Command data-transfer object
    /// </summary>
    public class CommandDTO : BaseDTO
    {
        /// <summary>
        /// The text that can be used from any inbound route to trigger a Command Reply
        /// </summary>
        public string CommandText { get; set; }
        /// <summary>
        /// The description of the Command
        /// </summary>
        public string Description { get; set; }
    }
}
