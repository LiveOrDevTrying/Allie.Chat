using System;
using System.ComponentModel.DataAnnotations;

namespace Allie.Chat.Lib.Requests.Commands
{
    /// <summary>
    /// A Command update request
    /// </summary>
    public class CommandUpdateRequest : BaseRequest
    {
        /// <summary>
        /// The Id of the existing command
        /// </summary>
        public Guid Id { get; set; }
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
