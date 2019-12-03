using System;

namespace Allie.Chat.Lib.DTOs.Servers.Channels
{
    /// <summary>
    /// A base Server Channel data-transfer object
    /// </summary>
    public class ServerChannelDTO : BaseDTO
    {
        /// <summary>
        /// The name of the Server Channel
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The Id of the associated Server
        /// </summary>
        public Guid ServerId { get; set; }
    }
}
