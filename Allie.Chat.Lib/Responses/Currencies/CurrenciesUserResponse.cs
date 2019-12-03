using Allie.Chat.Lib.DTOs.Currencies;
using Allie.Chat.Lib.DTOs.Users;

namespace Allie.Chat.Lib.Responses.Currencies
{
    /// <summary>
    /// A Currencies Users response
    /// </summary>
    public class CurrenciesUserResponse
    {
        /// <summary>
        /// The User that owns the User Currencies
        /// </summary>
        public UserDTO User { get; set; }
        /// <summary>
        /// The User's Currencies
        /// </summary>
        public CurrencyUserDTO[] CurrenciesUser { get; set; }
    }
}
