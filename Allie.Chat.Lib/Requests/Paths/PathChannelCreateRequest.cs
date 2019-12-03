using System;
using System.ComponentModel.DataAnnotations;

namespace Allie.Chat.Lib.Requests.Paths
{
    /// <summary>
    /// A Path Channel create request
    /// </summary>
    public class PathChannelCreateRequest : PathServerCreateRequest
    {
        /// <summary>
        /// The Id of the Channel to register to the Path
        /// </summary>
        [Required]
        public Guid ChannelId { get; set; }
    }
}
