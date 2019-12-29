﻿using Allie.Chat.Commands.Core.Interfaces;

namespace Allie.Chat.Commands.Core.Models
{
    public struct ParametersCommandsToken : IParametersToken
    {
        public string BotAccessToken { get; set; }
        public string WebAPIToken { get; set; }
        public int StreamCachePollingIntervalMS { get; set; }
        public int ReconnectPollingIntervalMS { get; set; }
    }
}