using System;
using System.ComponentModel.DataAnnotations;

namespace Allie.Chat.Lib.Requests.Commands
{
    /// <summary>
    /// A Command Reply update request
    /// </summary>
    public class CommandReplyUpdateRequest : UpdateRequest
    {
        /// <summary>
        /// The Command Reply text to send to the outbound streams when a Command is triggered
        /// </summary>
        /// [Required]
        public string CommandReplyText { get; set; }
    }
}
