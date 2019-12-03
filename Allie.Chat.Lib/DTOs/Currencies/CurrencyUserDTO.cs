namespace Allie.Chat.Lib.DTOs.Currencies
{
    /// <summary>
    /// A Currency User data-transfer object
    /// </summary>
    public class CurrencyUserDTO : BaseDTO
    {
        /// <summary>
        /// The amount of Currency currently accrued
        /// </summary>
        public int Quantity { get; set; }
    }
}
