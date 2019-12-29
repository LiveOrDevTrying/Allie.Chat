﻿using Allie.Chat.Commands.Core.Interfaces;

namespace Allie.Chat.Commands.Core.Models
{
    public struct ParametersCommands : IParameters
    {
        public string BotAccessToken { get; set; }
        public string WebAPIToken { get; set; }
    }
}
