using Allie.Chat.Lib.DTOs.Routes;
using Allie.Chat.Lib.DTOs.Users;
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

        /// <summary>
        /// The User registered to the Bot
        /// </summary>
        public UserDTO User { get; set; }
    }
}
