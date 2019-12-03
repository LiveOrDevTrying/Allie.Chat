using Allie.Chat.Lib.Enums;

namespace Allie.Chat.Lib.DTOs.Paths
{
    /// <summary>
    /// A Path data-transfer object
    /// </summary>
    public class PathDTO : BaseDTO
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
    }
}
