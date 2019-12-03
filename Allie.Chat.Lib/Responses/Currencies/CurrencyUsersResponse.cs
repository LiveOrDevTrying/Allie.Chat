using System;

namespace Allie.Chat.Lib.Responses.Currencies
{
    /// <summary>
    /// A Currency Users response
    /// </summary>
    public class CurrencyUsersResponse
    {
        /// <summary>
        /// The Currency User Ids that could not be located
        /// </summary>
        public string[] RejectedIds { get; set; }
        /// <summary>
        /// The requested Currency Users
        /// </summary>
        public CurrencyUserResponse[] CurrencyUsers { get; set; }
    }
}
