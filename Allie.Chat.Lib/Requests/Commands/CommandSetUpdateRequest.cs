using System.ComponentModel.DataAnnotations;

namespace Allie.Chat.Lib.Requests.Commands
{
    /// <summary>
    /// A Command Set update request
    /// </summary>
    public class CommandSetUpdateRequest : UpdateRequest
    {
        /// <summary>
        /// The name of the Command Set
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// The description of the Command Set
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// The prefix that can be used from any inbound route that starts the Command and Command Reply
        /// </summary>
        [Required]
        public string Prefix { get; set; }
    }
}
