namespace Allie.Chat.Commands.Tcp.Auth.Interfaces
{
    public interface IParametersBaseTcpAuth
    {
        string BotAccessToken { get; set; }
        int StreamCachePollingIntervalMS { get; set; }
        int ReconnectPollingIntervalMS { get; set; }
    }
}
