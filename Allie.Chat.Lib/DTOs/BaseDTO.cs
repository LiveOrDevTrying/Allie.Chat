using System;

namespace Allie.Chat.Lib.DTOs
{
    /// <summary>
    /// A base data-transfer object
    /// </summary>
    public abstract class BaseDTO
    {
        /// <summary>
        /// The unique Id
        /// </summary>
        public Guid Id { get; set; }
    }
}
