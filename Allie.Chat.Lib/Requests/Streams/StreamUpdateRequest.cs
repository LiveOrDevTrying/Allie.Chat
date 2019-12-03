using System.ComponentModel.DataAnnotations;

namespace Allie.Chat.Lib.Requests.Streams
{
    /// <summary>
    /// A Stream update request
    /// </summary>
    public class StreamUpdateRequest : UpdateRequest
    {
        /// <summary>
        /// The name of the Stream
        /// </summary>
        [Required]
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
