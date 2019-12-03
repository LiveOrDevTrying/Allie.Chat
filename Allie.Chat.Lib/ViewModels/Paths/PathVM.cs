using Allie.Chat.Lib.DTOs.Routes;
using Allie.Chat.Lib.Enums;

namespace Allie.Chat.Lib.ViewModels.Paths
{
    /// <summary>
    /// A base Path ViewModel
    /// </summary>
    public class PathVM : BaseVM
    {
        /// <summary>
        /// The Path type
        /// Values: 0 - None, 1 - Server, 2 - Channel
        /// </summary>
        public PathType PathType { get; set; }
        /// <summary>
        /// The Path type
        /// Values: "None", "Server", "Channel"
        /// </summary>
        public string PathTypeValue { get; set; }
        /// <summary>
        /// The Route that the Path is registered
        /// </summary>
        public RouteDTO Route { get; set; }
    }
}
