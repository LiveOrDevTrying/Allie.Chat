using Allie.Chat.Commands.Tcp.Auth.Interfaces;

namespace Allie.Chat.Commands.Tcp.Auth.Models
{
    public struct ParametersTcpAuthROPassword : IParametersTcpAuthROPassword
    {
        public string BotAccessToken { get; set; }
        public int StreamCachePollingIntervalMS { get; set; }
        public int ReconnectPollingIntervalMS { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scopes { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
