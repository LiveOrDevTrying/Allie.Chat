using Allie.Chat.Lib.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Allie.Chat.Lib.Requests.Routes
{
    /// <summary>
    /// A Route create request
    /// </summary>
    public class RouteCreateRequest : BaseRequest
    {
        /// <summary>
        /// The Id of the Stream to register to the Route
        /// </summary>
        [Required]
        public Guid StreamId { get; set; }
        /// <summary>
        /// The Id of the Bot to register to the Route
        /// </summary>
        [Required]
        public Guid BotId { get; set; }
        /// <summary>
        /// The Route type
        /// Values: 0 - Inbound, 1 - Outbound
        /// </summary>
        [Required]
        public RouteType RouteType { get; set; }
    }
}
