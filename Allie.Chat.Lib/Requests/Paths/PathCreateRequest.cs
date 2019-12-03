using System;
using System.ComponentModel.DataAnnotations;

namespace Allie.Chat.Lib.Requests.Paths
{
    /// <summary>
    /// A base Path create request
    /// </summary>
    public class PathCreateRequest : BaseRequest
    {
        /// <summary>
        /// The Id of the Route to register the path
        /// </summary>
        [Required]
        public Guid RouteId { get; set; }
    }
}
