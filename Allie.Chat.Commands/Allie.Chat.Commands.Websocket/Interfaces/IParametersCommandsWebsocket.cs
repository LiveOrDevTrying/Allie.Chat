namespace Allie.Chat.Commands.Websocket.Interfaces
{
    public interface IParametersCommandsWebsocket
    {
        string BotAccessToken { get; set; }
        string WebAPIToken { get; set; }
        int StreamCachePollingIntervalMS { get; set; }
        int ReconnectPollingIntervalMS { get; set; }
    }
}
