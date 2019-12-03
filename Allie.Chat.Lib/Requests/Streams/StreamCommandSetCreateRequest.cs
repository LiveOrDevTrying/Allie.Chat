using System;
using System.ComponentModel.DataAnnotations;

namespace Allie.Chat.Lib.Requests.Commands
{
    /// <summary>
    /// A StreamCommandSet create request
    /// </summary>
    public class StreamCommandSetCreateRequest
    {
        /// <summary>
        /// The Id of the Stream to register the Command Set
        /// </summary>
        [Required]
        public Guid StreamId { get; set; }
        /// <summary>
        /// The Id of the Command Set to register to the Stream
        /// </summary>
        [Required]
        public Guid CommandSetId { get; set; }
    }
}
