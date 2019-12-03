using Allie.Chat.Lib.DTOs.Commands;
using System.Collections.Generic;

namespace Allie.Chat.Lib.ViewModels.Commands
{
    /// <summary>
    /// A Command Set ViewModel
    /// </summary>
    public class CommandSetVM : BaseVM
    {
        /// <summary>
        /// The name of the Command Set
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The description of the Command Set
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// The prefix that can be used from any inbound route that starts the Command and Command Reply
        /// </summary>
        public string Prefix { get; set; }
        /// <summary>
        /// The Commands that are registered to this Command Set
        /// </summary>
        public ICollection<CommandDTO> Commands { get; set; } =
            new List<CommandDTO>();
    }
}
