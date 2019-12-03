using Allie.Chat.Lib.DTOs.Servers.Channels;
using System.Collections.Generic;

namespace Allie.Chat.Lib.ViewModels.Servers
{
    /// <summary>
    /// A Discord Server ViewModel
    /// </summary>
    public class ServerDiscordVM : ServerVM
    {
        /// <summary>
        /// The Discord Channels in the Server
        /// </summary>
        public ICollection<ServerChannelDiscordDTO> Channels { get; set; } =
            new List<ServerChannelDiscordDTO>();

    }
}