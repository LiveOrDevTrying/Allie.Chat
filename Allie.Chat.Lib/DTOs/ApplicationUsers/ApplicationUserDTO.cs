namespace Allie.Chat.Lib.DTOs.ApplicationUsers
{
    /// <summary>
    /// The Application User data-transfer object
    /// </summary>
    public class ApplicationUserDTO : BaseDTO
    {
        /// <summary>
        /// Is this user active?
        /// </summary>
        public bool Active { get; set; } = true;
        /// <summary>
        /// The Application User's username
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// The Application User's maximum number of streams
        /// </summary>
        public int MaxStreamCount { get; set; }
        /// <summary>
        /// The Application User's maximum number of bots per provider
        /// </summary>
        public int MaxBotCountPerProvider { get; set; }
        /// <summary>
        /// The Application User's maximum number of currencies
        /// </summary>
        public int MaxCurrencies { get; set; }
        /// <summary>
        /// The Application User's maximum number of command sets
        /// </summary>
        public int MaxCommandSets { get; set; }
    }
}
