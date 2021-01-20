using Allie.Chat.Auth.Interfaces;

namespace Allie.Chat.Auth.Models
{
    public struct ParametersAuthPKCE : IParametersAuthPKCE
    {
        public string BotAccessToken { get; set; }
        public int StreamCachePollingIntervalMS { get; set; }
        public int ReconnectPollingIntervalMS { get; set; }
        public string ClientId { get; set; }
        public string Scopes { get; set; }
    }
}
