using Allie.Chat.Commands.Core;
using Allie.Chat.Commands.Core.Auth;
using Allie.Chat.Commands.Core.Interfaces;
using Allie.Chat.Lib.ViewModels.Bots;
using Allie.Chat.Websocket;
using System.Threading.Tasks;
using WebsocketsSimple.Core.Events.Args;

namespace Allie.Chat.Commands.Websocket.Auth
{
    public abstract class BaseCommandsWebsocketAuth : BaseCommandsAuthService, ICommandsService
    {
        protected IWSClientAC _wsClient;
        protected int _reconnectPollingIntervalIndex;
        protected bool _isRunning;

        private const int RECONNECT_POLLING_INTERVAL_MS = 15000;

        public BaseCommandsWebsocketAuth(IParameters parameters)
            : base(parameters)
        {
            Task.Run(async () =>
            {
                await GetAccessTokenAsync();
            });
        }

        public override async Task SendMessageAsync(string message)
        {
            await _wsClient.SendAsync(message);
        }

        protected abstract Task GetAccessTokenAsync();

        protected virtual async Task ConnectAsync()
        {
            Disconnect();

            _wsClient = new WSClientAC(_parameters.BotAccessToken);
            _wsClient.MessageEvent += OnMessageEvent;
            _wsClient.ConnectionEvent += OnConnectionEvent;
            _wsClient.ErrorEvent += OnErrorEvent;

            await _wsClient.ConnectAsync();
            _isRunning = true;
        }

        protected virtual void Disconnect()
        {
            _isRunning = false;

            if (_wsClient != null)
            {
                _wsClient.MessageEvent -= OnMessageEvent;
                _wsClient.ConnectionEvent -= OnConnectionEvent;
                _wsClient.ErrorEvent -= OnErrorEvent;
                _wsClient.Dispose();
            }
        }
        protected override async Task UpdateEventsAsync(int updateIntervalMS)
        {
            if (_isRunning && _bot != null)
            {
                _reconnectPollingIntervalIndex += updateIntervalMS;

                if (_reconnectPollingIntervalIndex >= RECONNECT_POLLING_INTERVAL_MS)
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

        protected virtual Task OnErrorEvent(object sender, WSErrorEventArgs args)
        {
            FireErrorEvent(sender, args);
            return Task.CompletedTask;
        }
        protected virtual Task OnConnectionEvent(object sender, WSConnectionEventArgs args)
        {
            FireConnectionEvent(sender, args);
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            Disconnect();

            base.Dispose();
        }
    }
}
