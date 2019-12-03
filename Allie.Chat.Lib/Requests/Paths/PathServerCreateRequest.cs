using System;
using System.ComponentModel.DataAnnotations;

namespace Allie.Chat.Lib.Requests.Paths
{
    /// <summary>
    /// A Path Server create request
    /// </summary>
    public class PathServerCreateRequest : PathCreateRequest
    {
        /// <summary>
        /// The Id of the Server to register the Path
        /// </summary>
        [Required]
        public Guid ServerId { get; set; }
    }
}
