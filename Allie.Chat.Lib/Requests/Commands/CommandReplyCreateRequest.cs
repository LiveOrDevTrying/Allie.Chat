using System;
using System.ComponentModel.DataAnnotations;

namespace Allie.Chat.Lib.Requests.Commands
{
    /// <summary>
    /// A Command Reply create request
    /// </summary>
    public class CommandReplyCreateRequest : BaseRequest
    {
        /// <summary>
        /// The Command Reply text to send to the outbound streams when a Command is triggered
        /// </summary>
        [Required]
        public string CommandReplyText { get; set; }
        /// <summary>
        /// The Id of the Command to register the Command Reply
        /// </summary>
        [Required]
        public Guid CommandId { get; set; }
    }
}
