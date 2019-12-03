using Allie.Chat.Lib.DTOs;
using Allie.Chat.Lib.DTOs.Streams;

namespace Allie.Chat.Lib.ViewModels.Currencies
{
    /// <summary>
    /// A Stream Currency Provider ViewModel
    /// </summary>
    public class StreamCurrencyProviderVM : BaseVM
    {
        /// <summary>
        /// The Provider that is registered to this Stream Currency Provider
        /// </summary>
        public ProviderDTO Provider { get; set; }
        /// <summary>
        /// The StreamCurrency that is registered to this Stream Currency Provider
        /// </summary>
        public StreamCurrencyDTO StreamCurrency { get; set; }
    }
}
