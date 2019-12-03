namespace Allie.Chat.Lib.DTOs.Commands
{
    /// <summary>
    /// A Command Set data-transfer object
    /// </summary>
    public class CommandSetDTO : BaseDTO
    {
        /// <summary>
        /// The name of the Command Set
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The description of the Command Set
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// The prefix that can be used from any inbound route that starts the Command and Command Reply
        /// </summary>
        public string Prefix { get; set; }
        /// <summary>
        /// The number of Commands registered to the Command Set
        /// </summary>
        public int CommandsCount { get; set; }
        /// <summary>
        /// The number of Streams that the Command Set is registered
        /// </summary>
        public int StreamsCommandSets { get; set; }
    }
}
