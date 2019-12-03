namespace Allie.Chat.Lib.DTOs.Currencies
{
    /// <summary>
    /// A Currency data-transfer object
    /// </summary>
    public class CurrencyDTO : BaseDTO
    {
        /// <summary>
        /// The name of the Currency
        /// </summary>
        public string CurrencyName { get; set; }
        /// <summary>
        /// The description of the Currency
        /// </summary>
        public string CurrencyDescription { get; set; }
        /// <summary>
        /// The number of Streams the Currency is registered
        /// </summary>
        public int StreamCurrencyCount { get; set; }
    }
}
