using Allie.Chat.Commands.Core;
using Allie.Chat.Commands.Websocket.Interfaces;
using Allie.Chat.Lib.ViewModels.Bots;
using Allie.Chat.WebAPI.Auth;
using Allie.Chat.Websocket;
using System.Threading.Tasks;

namespace Allie.Chat.Commands.Websocket
{
    public class CommandsWebsocket : BaseCommandsService, ICommandsService
    {
        protected readonly IWSClientAC _wsClient;
        protected readonly IParametersCommandsWebsocket _parameters;
        protected int _reconnectPollingIntervalIndex;
        protected bool _isRunning;

        public CommandsWebsocket(IParametersCommandsWebsocket parameters)
            : base(parameters.WebAPIToken, parameters.StreamCachePollingIntervalMS, new WebAPIClientACAuth())
        {
            _parameters = parameters;

            _wsClient = new WSClientAC(_parameters.BotAccessToken);
            _wsClient.MessageEvent += OnMessageEvent;

            Task.Run(async () =>
            {
                await _wsClient.ConnectAsync();
                _isRunning = true;
            });
        }

        protected override async Task UpdateEventsAsync(int updateIntervalMS)
        {
            if (_isRunning && _bot != null)
            {
                _reconnectPollingIntervalIndex += updateIntervalMS;

                if (_reconnectPollingIntervalIndex >= _parameters.ReconnectPollingIntervalMS)
                {
                    _reconnectPollingIntervalIndex = 0;

                    if (_wsClient != null &&
                        !_wsClient.IsRunning)
                    {
                        await _wsClient.ConnectAsync();
                    }
                }
            }

            await base.UpdateEventsAsync(updateIntervalMS);
        }
        protected override async Task<BotVM> GetBotAsync()
        {
            return await _webapiClient.GetBotWebsocketAsync(_parameters.BotAccessToken);
        }

        public override void Dispose()
        {
            _isRunning = false;

            if (_wsClient != null)
            {
                _wsClient.MessageEvent -= OnMessageEvent;
                _wsClient.Dispose();
            }

            base.Dispose();
        }
    }
}
