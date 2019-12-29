using Allie.Chat.Commands.Enums;
using Allie.Chat.Commands.Interfaces;

namespace Allie.Chat.Commands.Models
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
