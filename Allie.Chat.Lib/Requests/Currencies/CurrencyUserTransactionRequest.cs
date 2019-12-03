using System;
using System.ComponentModel.DataAnnotations;

namespace Allie.Chat.Lib.Requests.Currencies
{
    /// <summary>
    /// A Currency User Transaction request 
    /// </summary>
    public class CurrencyUserTransactionRequest : BaseRequest
    {
        /// <summary>
        /// The Id of the Currency User
        /// </summary>
        public Guid CurrencyUserId { get; set; }
        /// <summary>
        /// The amount of Currency to reward / remove
        /// </summary>
        public int QuantityToAdjust { get; set; }
    }
}
