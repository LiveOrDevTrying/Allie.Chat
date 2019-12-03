using Allie.Chat.Lib.DTOs.Routes;
using Allie.Chat.Lib.DTOs.Streams;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Allie.Chat.Lib.ViewModels.Streams
{
    /// <summary>
    /// A Stream ViewModel
    /// </summary>
    public class StreamVM : BaseVM
    {
        /// <summary>
        /// The name of the Stream
        /// </summary>
        [Display(Name = "Stream Name")]
        public string StreamName { get; set; }
        /// <summary>
        /// Does the Stream have Currencies?
        /// </summary>
        [Display(Name = "Has Currency")]
        public bool HasCurrency { get; set; }
        /// <summary>
        /// Does the Stream have Commands?
        /// </summary>
        [Display(Name = "Has Commands")]
        public bool HasCommands { get; set; }
        /// <summary>
        /// The Routes registered to the Stream
        /// </summary>
        public ICollection<RouteDTO> Routes { get; set; } =
            new List<RouteDTO>();
        /// <summary>
        /// The Currencies registered to the Stream
        /// </summary>
        public ICollection<StreamCurrencyDTO> StreamsCurrencies { get; set; } =
            new List<StreamCurrencyDTO>();
        /// <summary>
        /// The Command Sets registered to the Stream
        /// </summary>
        public ICollection<StreamCommandSetDTO> StreamsCommandSets { get; set; } =
            new List<StreamCommandSetDTO>();
    }
}
