using System;
using System.ComponentModel.DataAnnotations;

namespace Allie.Chat.Lib.Requests.StreamsCurrencies
{
    /// <summary>
    /// A Status update request
    /// </summary>
    public class StatusUpdateRequest : UpdateRequest
    {
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
