using Allie.Chat.Lib.DTOs.Streams;
using System.Collections.Generic;

namespace Allie.Chat.Lib.ViewModels.Currencies
{
    /// <summary>
    /// A Currency ViewModel
    /// </summary>
    public class CurrencyVM : BaseVM
    {
        /// <summary>
        /// The name of the currency
        /// </summary>
        public string CurrencyName { get; set; }
        /// <summary>
        /// The description of the currency
        /// </summary>
        public string CurrencyDescription { get; set; }
        /// <summary>
        /// The StreamCurrencies of the Streams this Currency is registered
        /// </summary>
        public ICollection<StreamCurrencyDTO> StreamsCurrencies { get; set; } =
            new List<StreamCurrencyDTO>();
    }
}
