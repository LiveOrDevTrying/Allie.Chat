using Allie.Chat.Commands.Websocket.Interfaces;

namespace Allie.Chat.Commands.Websocket.Models
{
    public class ParametersCommandsWebsocket : IParametersCommandsWebsocket
    {
        public string BotAccessToken { get; set; }
        public string WebAPIToken { get; set; }
        public int StreamCachePollingIntervalMS { get; set; }
        public int ReconnectPollingIntervalMS { get; set; }
    }
}
