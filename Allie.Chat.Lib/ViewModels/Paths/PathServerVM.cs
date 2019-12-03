using Allie.Chat.Lib.DTOs.Servers;

namespace Allie.Chat.Lib.ViewModels.Paths
{
    /// <summary>
    /// A Path Server ViewModel
    /// </summary>
    public class PathServerVM : PathVM
    {
        /// <summary>
        /// The Server registered to the Path
        /// </summary>
        public ServerDTO Server { get; set; }
    }
}
