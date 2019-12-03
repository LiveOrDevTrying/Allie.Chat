using Allie.Chat.Lib.DTOs.Servers;

namespace Allie.Chat.Lib.Responses.Servers
{
    /// <summary>
    /// A base Servers response
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ServersBaseResponse<T> where T : ServerDTO
    {
        /// <summary>
        /// The requested Servers data-transfer objects
        /// </summary>
        public T[] Servers { get; set; }
        /// <summary>
        /// Ids that did not return a Server by TwitchChannelId or Name
        /// </summary>
        public string[] InvalidIds { get; set; }
    }
}
