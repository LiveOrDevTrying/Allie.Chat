using Allie.Chat.Commands.Tcp.Auth.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Allie.Chat.Commands.Tcp.Auth.Models
{
    public struct ParametersTcpAuthCode : IParametersTcpAuthCode
    {
        public string BotAccessToken { get; set; }
        public int StreamCachePollingIntervalMS { get; set; }
        public int ReconnectPollingIntervalMS { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scopes { get; set; }
    }
}
