using Allie.Chat.Lib.Enums;

namespace Allie.Chat.Lib.DTOs.Routes
{
    /// <summary>
    /// A Route data-transfer objecte
    /// </summary>
    public class RouteDTO : BaseDTO
    {
        /// <summary>
        /// The Route type
        /// Values: 0 - Inbound, 1 - Outbound
        /// </summary>
        public RouteType RouteType { get; set; }
        /// <summary>
        /// The Route type
        /// Values: "Inbound", "Outbound"
        /// </summary>
        public string RouteTypeValue { get; set; }
    }
}
