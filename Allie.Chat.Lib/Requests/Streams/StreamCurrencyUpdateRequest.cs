using System;
using System.ComponentModel.DataAnnotations;

namespace Allie.Chat.Lib.Requests.Streams
{
    /// <summary>
    /// A StreamCurrency update request
    /// </summary>
    public class StreamCurrencyUpdateRequest
    {
        /// <summary>
        /// The Id of the existing StreamCurrency
        /// </summary>
        public string Id { get; set; }
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
        /// Array of Provider Ids that accrue Currency
        /// </summary>
        public Guid[] ProviderIds { get; set; }
    }
}
