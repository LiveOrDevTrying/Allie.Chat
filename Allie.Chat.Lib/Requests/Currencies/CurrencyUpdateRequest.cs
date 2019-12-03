using System.ComponentModel.DataAnnotations;

namespace Allie.Chat.Lib.Requests.Currencies
{
    /// <summary>
    /// A Currency update request
    /// </summary>
    public class CurrencyUpdateRequest : UpdateRequest
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
