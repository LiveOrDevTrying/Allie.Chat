using System.Collections.Generic;
using Allie.Chat.Lib.DTOs.Streams;
using Allie.Chat.Lib.DTOs.Bots;
using Allie.Chat.Lib.DTOs.Paths;
using Allie.Chat.Lib.Enums;

namespace Allie.Chat.Lib.ViewModels.Routes
{
    /// <summary>
    /// A Route ViewModel
    /// </summary>
    public class RouteVM : BaseVM
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
        /// <summary>
        /// The Stream that is registered to the Route
        /// </summary>
        public StreamDTO Stream { get; set; }
        /// <summary>
        /// The Bot that is registered to the Route
        /// </summary>
        public BotDTO Bot { get; set; }
        /// <summary>
        /// The Paths registered to the Route
        /// </summary>
        public ICollection<PathDTO> Paths { get; set; } =
            new List<PathDTO>();
    }
}
