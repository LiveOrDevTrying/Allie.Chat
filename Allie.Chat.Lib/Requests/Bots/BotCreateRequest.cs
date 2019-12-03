using System;

namespace Allie.Chat.Lib.Requests.Bots
{
    /// <summary>
    /// A base Bot create request
    /// </summary>
    public abstract class BotCreateRequest : BaseRequest
    {
        /// <summary>
        /// The UserId of the Bot User
        /// </summary>
        public Guid UserId { get; set; }
    }
}
