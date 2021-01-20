using Allie.Chat.Enums;
using Allie.Chat.Interfaces;

namespace Allie.Chat.Models
{
    public struct ACParametersAuthPKCE : IACParametersAuthPKCE
    {
        public ClientType ClientType { get; set; }
        public string ClientId { get; set; }
        public string Scopes { get; set; }
        public string BotAccessToken { get; set; }
        public int StreamCachePollingIntervalMS { get; set; }
        public int ReconnectPollingIntervalMS { get; set; }
    }
}
