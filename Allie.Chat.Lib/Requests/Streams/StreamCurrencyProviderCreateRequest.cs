using System;
using System.ComponentModel.DataAnnotations;

namespace Allie.Chat.Lib.Requests.Streams
{
    /// <summary>
    /// A StreamCurrencyProvider create request
    /// </summary>
    public class StreamCurrencyProviderCreateRequest
    {
        /// <summary>
        /// The Id of the Currency Stream to register the Provider
        /// </summary>
        [Required]
        public Guid StreamCurrencyId { get; set; }
        /// <summary>
        /// The Id of the Provider to register to the Currency Stream
        /// </summary>
        [Required]
        public Guid ProviderId { get; set; }
    }
}
