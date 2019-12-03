using System.ComponentModel.DataAnnotations;

namespace Allie.Chat.Lib.DTOs.Servers
{
    /// <summary>
    /// A Twitch Server data-transfer object
    /// </summary>
    public class ServerTwitchDTO : ServerDTO
    {
        /// <summary>
        /// The Server's Twitch display name
        /// </summary>
        public string TwitchDisplayName { get; set; }
    }
}