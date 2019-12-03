using Allie.Chat.Lib.DTOs.Streams;

namespace Allie.Chat.Lib.ViewModels.StreamsCurrencies
{
    /// <summary>
    /// A Status ViewModel
    /// </summary>
    public class StatusVM : BaseVM
    {
        /// <summary>
        /// The name of the Status
        /// </summary>
        public string StatusName { get; set; }
        /// <summary>
        /// Time in seconds that elapses before this Status becomes active
        /// </summary>
        public int TimeSecLastActivityBeforeStatus { get; set; }
        /// <summary>
        /// The amount of Currency accrued per interval when the User is in this Status
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// The Currency in the Stream this Status is registered
        /// </summary>
        public StreamCurrencyDTO StreamCurrency { get; set; }
    }
}
