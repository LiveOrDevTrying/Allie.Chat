using System.ComponentModel.DataAnnotations;

namespace Allie.Chat.Lib.Requests.Currencies
{
    /// <summary>
    /// A Currency create request 
    /// </summary>
    public class CurrencyCreateRequest : BaseRequest
    {
        /// <summary>
        /// The name of the Currency
        /// </summary>
        [Required]
        public string CurrencyName { get; set; }
        /// <summary>
        /// The description of the Currency
        /// </summary>
        public string CurrencyDescription { get; set; }
    }
}
