using System;
using System.Collections.Generic;
using System.Text;

namespace Allie.Chat.Commands.Tcp.Interfaces
{
    public interface IParametersCommandsTcp
    {
        string BotAccessToken { get; set; }
        string WebAPIToken { get; set; }
        int StreamCachePollingIntervalMS { get; set; }
        int ReconnectPollingIntervalMS { get; set; }
    }
}
