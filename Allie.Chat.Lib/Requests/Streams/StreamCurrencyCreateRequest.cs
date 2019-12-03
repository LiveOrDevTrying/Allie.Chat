using System;
using System.ComponentModel.DataAnnotations;

namespace Allie.Chat.Lib.Requests.Streams
{
    /// <summary>
    /// A StreamCurrency create request
    /// </summary>
    public class StreamCurrencyCreateRequest
    {
        /// <summary>
        /// Interval in Seconds that Currency will be added to the Stream
        /// </summary>
        [Required]
        public int TimeSec { get; set; }
        /// <summary>
        /// The Prefix that can be used from any inbound route to report a User's current currency
        /// </summary>
        [Required]
        public string Prefix { get; set; }
        /// <summary>
        /// The Id of the Stream to add the Currency 
        /// </summary>
        [Required]
        public Guid StreamId { get; set; }
        /// <summary>
        /// The Id of the Currency to add to the Stream
        /// </summary>
        [Required]
        public Guid CurrencyId { get; set; }
        /// <summary>
        /// Array of Provider Ids that accrue Currency
        /// </summary>
        public Guid[] ProviderIds { get; set; }
    }
}
