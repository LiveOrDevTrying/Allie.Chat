using System;

namespace Allie.Chat.Lib.DTOs.Streams
{
    /// <summary>
    /// A StreamCommandSet data-transfer object
    /// </summary>
    public class StreamCommandSetDTO : BaseDTO
    {
        /// <summary>
        /// The Id of the Command Set
        /// </summary>
        public Guid CommandSetId { get; set; }
    }
}
