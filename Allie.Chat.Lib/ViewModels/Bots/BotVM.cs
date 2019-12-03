using Allie.Chat.Lib.DTOs.ApplicationUsers;
using Allie.Chat.Lib.DTOs.Routes;
using Allie.Chat.Lib.Enums;
using System.Collections.Generic;

namespace Allie.Chat.Lib.ViewModels.Bots
{
    /// <summary>
    /// A base Bot ViewModel
    /// </summary>
    public class BotVM : BaseVM 
    {
        /// <summary>
        /// The Routes registered to the Bot
        /// </summary>
        public ICollection<RouteDTO> Routes { get; set; } =
            new List<RouteDTO>();
    }
}
