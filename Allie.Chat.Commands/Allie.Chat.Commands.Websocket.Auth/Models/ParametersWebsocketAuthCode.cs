using Allie.Chat.Commands.Websocket.Auth.Interfaces;

namespace Allie.Chat.Commands.Websocket.Auth.Models
{
    public struct ParametersWebsocketAuthCode : IParametersWebsocketAuthCode
    {
        public string BotAccessToken { get; set; }
        public string WebAPIToken { get; set; }
        public int StreamCachePollingIntervalMS { get; set; }
        public int ReconnectPollingIntervalMS { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scopes { get; set; }
    }
}
