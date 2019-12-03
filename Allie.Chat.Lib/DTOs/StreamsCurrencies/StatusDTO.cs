using System.ComponentModel.DataAnnotations;

namespace Allie.Chat.Lib.DTOs.StreamsCurrencies
{
    /// <summary>
    /// A Status data-transfer object
    /// </summary>
    public class StatusDTO : BaseDTO
    {
        /// <summary>
        /// The name of the Status
        /// </summary>
        [Display(Name = "Status Name")]
        public string StatusName { get; set; }
        /// <summary>
        /// Time in seconds that elapses before this Status becomes active
        /// </summary>
        [Display(Name = "Time (Sec) From Last Activity Before Status")]
        public int TimeSecLastActivityBeforeStatus { get; set; }
        /// <summary>
        /// The amount of Currency accrued per interval when the User is in this Status
        /// </summary>
        [Display(Name = "Quantity Per Interval")]
        public int Quantity { get; set; }
    }
}
