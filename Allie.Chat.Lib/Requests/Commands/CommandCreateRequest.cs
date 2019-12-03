using System;
using System.ComponentModel.DataAnnotations;

namespace Allie.Chat.Lib.Requests.Commands
{
    /// <summary>
    /// A Command create request
    /// </summary>
    public class CommandCreateRequest : BaseRequest
    {
        /// <summary>
        /// The Command Set Id to register the Command
        /// </summary>
        [Required]
        public Guid CommandSetId { get; set; }
        /// <summary>
        /// The text that can be used from any inbound route to trigger a Command Reply
        /// </summary>
        [Required]
        public string CommandText { get; set; }
        /// <summary>
        /// The description of the Command
        /// </summary>
        public string Description { get; set; }
    }
}
