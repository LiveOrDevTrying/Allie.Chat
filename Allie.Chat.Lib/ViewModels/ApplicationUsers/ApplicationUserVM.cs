using Allie.Chat.Lib.DTOs.Bots;
using Allie.Chat.Lib.DTOs.Commands;
using Allie.Chat.Lib.DTOs.Currencies;
using Allie.Chat.Lib.DTOs.Streams;
using Allie.Chat.Lib.DTOs.Users;
using System.Collections.Generic;

namespace Allie.Chat.Lib.ViewModels.ApplicationUsers
{
    /// <summary>
    /// The Application User ViewModel
    /// </summary>
    public class ApplicationUserVM : BaseVM
    {
        /// <summary>
        /// Is this user active?
        /// </summary>
        public bool Active { get; set; } = true;
        /// <summary>
        /// The Application User's username
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// The Application User's maximum number of streams
        /// </summary>
        public int MaxStreamCount { get; set; }
        /// <summary>
        /// The Application User's maximum number of bots per provider
        /// </summary>
        public int MaxBotCountPerProvider { get; set; }
        /// <summary>
        /// The Application User's maximum number of currencies
        /// </summary>
        public int MaxCurrencies { get; set; }
        /// <summary>
        /// The Application User's maximum number of command sets
        /// </summary>
        public int MaxCommandSets { get; set; }
        /// <summary>
        /// The Application User's maximum number of Client Applications
        /// </summary>
        public int MaxClientApplications { get; set; }
        /// <summary>
        /// The Application User's maximum number of Api Resources
        /// </summary>
        public int MaxApiResourceCount { get; set; }

        /// <summary>
        /// The Application User's registered Currencies
        /// </summary>
        public ICollection<CurrencyDTO> Currencies { get; set; } =
            new List<CurrencyDTO>();

        /// <summary>
        /// The Application User's registered Streams
        /// </summary>
        public ICollection<StreamDTO> Streams { get; set; } =
            new List<StreamDTO>();

        /// <summary>
        /// The Application User's registered Bots
        /// </summary>
        public ICollection<BotDTO> Bots { get; set; } =
            new List<BotDTO>();

        /// <summary>
        /// The Application User's registered Command Sets
        /// </summary>
        public ICollection<CommandSetDTO> CommandSets { get; set; } = new
            List<CommandSetDTO>();

        /// <summary>
        /// The Application User's registered Users
        /// </summary>
        public ICollection<UserDTO> Users { get; set; } =
            new List<UserDTO>();
    }
}
