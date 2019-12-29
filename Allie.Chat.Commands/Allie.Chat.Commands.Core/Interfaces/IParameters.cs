namespace Allie.Chat.Commands.Core.Interfaces
{
    public interface IParameters
    {
        string BotAccessToken { get; set; }
        int StreamCachePollingIntervalMS { get; set; }
        int ReconnectPollingIntervalMS { get; set; }
    }
}
