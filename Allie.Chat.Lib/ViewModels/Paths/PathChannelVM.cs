using Allie.Chat.Lib.DTOs.Servers.Channels;

namespace Allie.Chat.Lib.ViewModels.Paths
{
    /// <summary>
    /// A Path Channel ViewModel
    /// </summary>
    public class PathChannelVM : PathServerVM
    {
        /// <summary>
        /// The Channel registered to the Path
        /// </summary>
        public ServerChannelDTO ServerChannel { get; set; }
    }
}
