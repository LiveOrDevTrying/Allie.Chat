using Allie.Chat.Lib.DTOs.Currencies;
using Allie.Chat.Lib.DTOs.Users;

namespace Allie.Chat.Lib.ViewModels.Currencies
{
    /// <summary>
    /// A Currency User ViewModel
    /// </summary>
    public class CurrencyUserVM : BaseVM
    {
        /// <summary>
        /// The Currency the User is accruing
        /// </summary>
        public CurrencyDTO Currency { get; set; }
        /// <summary>
        /// The User that is accruing the currency
        /// </summary>
        public UserDTO User { get; set; }
        /// <summary>
        /// The amount of Currency currently accrued
        /// </summary>
        public int Quantity { get; set; }
    }
}
