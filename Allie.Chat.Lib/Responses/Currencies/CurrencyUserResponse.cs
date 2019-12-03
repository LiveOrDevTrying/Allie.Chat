using Allie.Chat.Lib.DTOs.Currencies;
using Allie.Chat.Lib.DTOs.Users;

namespace Allie.Chat.Lib.Responses.Currencies
{
    /// <summary>
    /// A Currency User response
    /// </summary>
    public class CurrencyUserResponse
    {
        /// <summary>
        /// The User that owns the User User Currency
        /// </summary>
        public UserDTO User { get; set; }
        /// <summary>
        /// The User's Currency User
        /// </summary>
        public CurrencyUserDTO CurrencyUser { get; set; }
    }
}
