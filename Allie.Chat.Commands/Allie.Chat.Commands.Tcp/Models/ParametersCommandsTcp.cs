using Allie.Chat.Commands.Tcp.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Allie.Chat.Commands.Tcp.Models
{
    public class ParametersCommandsTcp : IParametersCommandsTcp
    {
        public string BotAccessToken { get; set; }
        public string WebAPIToken { get; set; }
        public int StreamCachePollingIntervalMS { get; set; }
        public int ReconnectPollingIntervalMS { get; set; }
    }
}
