using System;

namespace Allie.Chat.Lib.Requests
{
    /// <summary>
    /// A base Update request
    /// </summary>
    public abstract class UpdateRequest : BaseRequest
    {
        /// <summary>
        /// The Id of the existing object
        /// </summary>
        public Guid Id { get; set; }
    }
}
