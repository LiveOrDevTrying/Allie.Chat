namespace Allie.Chat.Lib.DTOs.Servers.Channels
{
    /// <summary>
    /// A Discord Server Channel data-transfer object
    /// </summary>
    public class ServerChannelDiscordDTO : ServerChannelDTO
    {
        /// <summary>
        /// The Id of the Channel from Discord
        /// </summary>
        public string DiscordChannelId { get; set; }
    }
}
