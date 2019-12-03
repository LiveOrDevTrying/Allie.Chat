using Allie.Chat.Lib.DTOs.Currencies;
using Allie.Chat.Lib.DTOs.StreamsCurrencies;
using Allie.Chat.Lib.DTOs.Streams;
using Allie.Chat.Lib.ViewModels.Currencies;
using System.Collections.Generic;

namespace Allie.Chat.Lib.ViewModels.Streams
{
    /// <summary>
    /// A StreamCurrency ViewModel
    /// </summary>
    public class StreamCurrencyVM : BaseVM
    {
        /// <summary>
        /// Interval in Seconds that Currency will be added to the Stream
        /// </summary>
        public int TimeSec { get; set; }
        /// <summary>
        /// The prefix that can be used from any inbound route to report a User's current currency
        /// </summary>
        public string Prefix { get; set; }
        /// <summary>
        /// The Currency registered to the Stream
        /// </summary>
        public CurrencyDTO Currency { get; set; }
        /// <summary>
        /// The Stream that contains the Currency
        /// </summary>
        public StreamDTO Stream { get; set; }
        /// <summary>
        /// Status registered to the stream
        /// </summary>
        public ICollection<StatusDTO> Status { get; set; } =
            new List<StatusDTO>();
        /// <summary>
        /// Providers in the Stream that accrue Currency
        /// </summary>
        public ICollection<StreamCurrencyProviderVM> StreamCurrencyProviders { get; set; } =
            new List<StreamCurrencyProviderVM>();

    }
}
