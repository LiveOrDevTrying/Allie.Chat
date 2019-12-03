namespace Allie.Chat.Lib.DTOs.Streams
{
    /// <summary>
    /// A Stream data-transfer object
    /// </summary>
    public class StreamDTO : BaseDTO
    {
        /// <summary>
        /// The name of the Stream
        /// </summary>
        public string StreamName { get; set; }
        /// <summary>
        /// Does the Stream have Currencies?
        /// </summary>
        public bool HasCurrency { get; set; }
        /// <summary>
        /// Does the Stream have Commands?
        /// </summary>
        public bool HasCommands { get; set; }
    }
}
