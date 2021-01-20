using Allie.Chat.Auth.Interfaces;

namespace Allie.Chat.Auth.Models
{
    public struct ParametersAuthCode : IParametersAuthCode
    {
        public string BotAccessToken { get; set; }
        public int StreamCachePollingIntervalMS { get; set; }
        public int ReconnectPollingIntervalMS { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scopes { get; set; }
    }
}
