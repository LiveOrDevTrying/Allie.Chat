using Allie.Chat.Commands.Enums;
using Allie.Chat.Commands.Interfaces;

namespace Allie.Chat.Commands.Models
{
    public struct ACParametersAuthROPassword : IACParametersAuthROPassword
    {
        public ClientType ClientType { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scopes { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string BotAccessToken { get; set; }
        public int StreamCachePollingIntervalMS { get; set; }
        public int ReconnectPollingIntervalMS { get; set; }
    }
}
