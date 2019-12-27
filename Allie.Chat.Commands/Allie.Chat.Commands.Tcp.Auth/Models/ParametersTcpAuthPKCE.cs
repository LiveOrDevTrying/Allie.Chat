using Allie.Chat.Commands.Tcp.Auth.Interfaces;

namespace Allie.Chat.Commands.Tcp.Auth.Models
{
    public struct ParametersTcpAuthPKCE : IParametersTcpAuthPKCE
    {
        public string BotAccessToken { get; set; }
        public int StreamCachePollingIntervalMS { get; set; }
        public int ReconnectPollingIntervalMS { get; set; }
        public string ClientId { get; set; }
        public string Scopes { get; set; }
    }
}
