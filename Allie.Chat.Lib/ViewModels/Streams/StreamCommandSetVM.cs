using Allie.Chat.Lib.DTOs.Commands;
using Allie.Chat.Lib.DTOs.Streams;

namespace Allie.Chat.Lib.ViewModels.Streams
{
    /// <summary>
    /// A StreamCommandSet ViewModel
    /// </summary>
    public class StreamCommandSetVM : BaseVM
    {
        /// <summary>
        /// The Command Set registered to the Stream
        /// </summary>
        public CommandSetDTO CommandSet { get; set; }
        /// <summary>
        /// The Stream that contains the registered Command Set
        /// </summary>
        public StreamDTO Stream { get; set; }
    }
}
