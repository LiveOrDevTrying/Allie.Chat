using System;
using System.ComponentModel.DataAnnotations;

namespace Allie.Chat.Lib.Requests.Currencies
{
    /// <summary>
    /// A Currency Users Transaction request 
    /// </summary>
    public class CurrencyUsersTransactionRequest : BaseRequest
    {
        /// <summary>
        /// The Ids of the Currencies Users to be updated
        /// </summary>
        public Guid[] CurrencyUserIds { get; set; }
        /// <summary>
        /// The amount of Currency to reward / remove
        /// </summary>
        public int QuantityToAdjust { get; set; }
    }
}
