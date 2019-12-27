namespace Allie.Chat.Commands.Websocket.Auth.Interfaces
{
    public interface IParametersCommandsBaseWebsocketAuth
    {
        string BotAccessToken { get; set; }
        string WebAPIToken { get; set; }
        int StreamCachePollingIntervalMS { get; set; }
        int ReconnectPollingIntervalMS { get; set; }
    }
}
