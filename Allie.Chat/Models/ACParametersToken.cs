using Allie.Chat.Enums;
using Allie.Chat.Interfaces;

namespace Allie.Chat.Models
{
    public struct ACParametersToken : IACParametersToken
    {
        public ClientType ClientType { get; set; }
        public string WebAPIToken { get; set; }
        public string BotAccessToken { get; set; }
        public int StreamCachePollingIntervalMS { get; set; }
        public int ReconnectPollingIntervalMS { get; set; }
    }
}
