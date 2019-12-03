using System;
using System.ComponentModel.DataAnnotations;

namespace Allie.Chat.Lib.Requests.StreamsCurrencies
{
    /// <summary>
    /// A Status create request
    /// </summary>
    public class StatusCreateRequest : BaseRequest
    {
        /// <summary>
        /// The Id of the Currency Stream to register this Status
        /// </summary>
        [Required]
        public Guid StreamCurrencyId { get; set; }
        /// <summary>
        /// The name of the Status
        /// </summary>
        [Required]
        public string StatusName { get; set; }
        /// <summary>
        /// Time in seconds that elapses before this Status becomes active
        /// </summary>
        [Required]
        public int TimeSecLastActivityBeforeStatus { get; set; }
        /// <summary>
        /// The amount of Currency accrued per interval when the User is in this Status
        /// </summary>
        [Required]
        public int Quantity { get; set; }
    }
}
