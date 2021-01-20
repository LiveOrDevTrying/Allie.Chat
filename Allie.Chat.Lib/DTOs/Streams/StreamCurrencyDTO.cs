using System;
using System.ComponentModel.DataAnnotations;

namespace Allie.Chat.Lib.DTOs.Streams
{
    /// <summary>
    /// A StreamCurrency data-transfer object
    /// </summary>
    public class StreamCurrencyDTO : BaseDTO
    {
        /// <summary>
        /// Interval in Seconds that Currency will be added to the Stream
        /// </summary>
        [Display(Name = "Interval (Sec) to Add Currency")]
        public int TimeSec { get; set; }
        /// <summary>
        /// The Prefix that can be used from any inbound route to report a User's current currency
        /// </summary>
        public string Prefix { get; set; }
        /// <summary>
        /// The Providers registered to the Stream Currency to accrue Currency 
        /// </summary>
        public string[] Providers { get; set; }
        /// <summary>
        /// The Statuses registered to the Stream Currency
        /// </summary>
        public string[] Status { get; set; }
        /// <summary>
        /// The number of Streams that the Currency is registered
        /// </summary>
        public int NumberStreamsCurrencyRegistered { get; set; }
    }
}
