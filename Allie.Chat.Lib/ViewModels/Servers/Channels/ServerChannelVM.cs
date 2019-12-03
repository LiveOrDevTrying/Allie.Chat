using Allie.Chat.Lib.DTOs;
using Allie.Chat.Lib.DTOs.Paths;
using Allie.Chat.Lib.DTOs.Servers;
using System.Collections.Generic;

namespace Allie.Chat.Lib.ViewModels.Servers.Channels
{
    /// <summary>
    /// A base Channel ViewModel
    /// </summary>
    public abstract class ServerChannelVM : BaseVM
    {
        /// <summary>
        /// The name of the Channel
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The Server that the Channel is registered
        /// </summary>
        public ServerDTO Server { get; set; }
    }
}
