using Allie.Chat.Commands.Core;
using Allie.Chat.Commands.Core.Interfaces;
using Allie.Chat.Lib.ViewModels.Bots;
using Allie.Chat.WebAPI.Auth;
using Allie.Chat.Websocket;
using System.Threading.Tasks;

namespace Allie.Chat.Commands.Websocket
{
    public class CommandsWebsocket : BaseCommandsTokenService, ICommandsService
    {
        protected readonly IWSClientAC _wsClient;
        protected int _reconnectPollingIntervalIndex;
        protected bool _isRunning;

        public CommandsWebsocket(IParametersToken parameters)
            : base(parameters)
        {
            _wsClient = new WSClientAC(_parameters.BotAccessToken);
            _wsClient.MessageEvent += OnMessageEvent;

            Task.Run(async () =>
            {
                _isRunning = true;
                await UpdateEventsAsync(0);
                await _wsClient.ConnectAsync();
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

        public override async Task SendMessageAsync(string message)
        {
            await _wsClient.SendAsync(message);
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
