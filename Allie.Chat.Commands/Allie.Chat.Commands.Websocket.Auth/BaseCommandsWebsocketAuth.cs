using Allie.Chat.Commands.Core;
using Allie.Chat.Commands.Core.Auth;
using Allie.Chat.Commands.Websocket.Auth.Interfaces;
using Allie.Chat.Lib.ViewModels.Bots;
using Allie.Chat.WebAPI.Auth;
using Allie.Chat.Websocket;
using System;
using System.Threading.Tasks;

namespace Allie.Chat.Commands.Websocket
{
    public abstract class BaseCommandsWebsocketAuth : BaseCommandsAuthService, ICommandsService
    {
        protected readonly IParametersCommandsBaseWebsocketAuth _parameters;
        protected IWSClientAC _wsClient;
        protected int _reconnectPollingIntervalIndex;
        protected bool _isRunning;

        public BaseCommandsWebsocketAuth(IParametersCommandsBaseWebsocketAuth parameters)
            : base(parameters.WebAPIToken, parameters.ReconnectPollingIntervalMS, new WebAPIClientACAuth())
        {
            _parameters = parameters;

            Task.Run(async () =>
            {
                await GetAccessTokenAsync();
            });
        }

        protected abstract Task GetAccessTokenAsync();

        protected virtual async Task ConnectAsync()
        {
            Disconnect();

            _wsClient = new WSClientAC(_parameters.BotAccessToken);
            _wsClient.MessageEvent += OnMessageEvent;

            await _wsClient.ConnectAsync();
            _isRunning = true;
        }
        protected virtual void Disconnect()
        {
            _isRunning = false;

            if (_wsClient != null)
            {
                _wsClient.MessageEvent -= OnMessageEvent;
                _wsClient.Dispose();
            }
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
            Disconnect();

            base.Dispose();
        }
    }
}
