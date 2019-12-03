using System;

namespace Allie.Chat.Lib.ViewModels
{
    /// <summary>
    /// A base ViewModel
    /// </summary>
    public abstract class BaseVM
    {
        /// <summary>
        /// The unique Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The Timestamp when this object was created or modified
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
