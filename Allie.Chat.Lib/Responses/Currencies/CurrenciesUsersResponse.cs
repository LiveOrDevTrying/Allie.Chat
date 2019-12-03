using System;

namespace Allie.Chat.Lib.Responses.Currencies
{
    /// <summary>
    /// A Currencies User response
    /// </summary>
    public class CurrenciesUsersResponse
    {
        /// <summary>
        /// The User and Currencies that owns the User User Currency
        /// </summary>
        public CurrenciesUserResponse[] CurrencyUsers;

        /// <summary>
        /// Ids 
        /// </summary>
        public string[] RejectedIds { get; set; }
    }
}
