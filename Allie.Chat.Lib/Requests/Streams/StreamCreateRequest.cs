using System.ComponentModel.DataAnnotations;

namespace Allie.Chat.Lib.Requests.Streams
{
    /// <summary>
    /// A Stream create request
    /// </summary>
    public class StreamCreateRequest : BaseRequest
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
