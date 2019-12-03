using System;

namespace Allie.Chat.Lib.Requests.Bots
{
    /// <summary>
    /// A base Bot update request
    /// </summary>
    public abstract class BotUpdateRequest : BaseRequest
    {
        /// <summary>
        /// The Id of the existing Bot
        /// </summary>
        public string Id { get; set; }
    }
}
