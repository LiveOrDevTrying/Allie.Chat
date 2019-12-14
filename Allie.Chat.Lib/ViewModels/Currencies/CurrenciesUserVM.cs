using Allie.Chat.Lib.DTOs.Currencies;
using Allie.Chat.Lib.DTOs.Users;

namespace Allie.Chat.Lib.ViewModels.Currencies
{
    /// <summary>
    /// A Currencies User ViewModel
    /// </summary>
    public class CurrenciesUserVM : BaseVM
    {
        /// <summary>
        /// The User that is accruing the stream currencies
        /// </summary>
        public UserDTO User { get; set; }
        /// <summary>
        /// The Stream Currencies the User is accruing
        /// </summary>
        public UserCurrency[] UserCurrencies { get; set; }

        /// <summary>
        /// A User Currency nested data-transfer object
        /// </summary>
        public class UserCurrency
        {
            /// <summary>
            /// The Currency the User is accruing
            /// </summary>
            public CurrencyDTO Currency { get; set; }

            /// <summary>
            /// The amount of Currency currently accrued
            /// </summary>
            public int Quantity { get; set; }
        }
    }
}
