using Allie.Chat.Commands.Core;
using Allie.Chat.Commands.Core.Interfaces;
using Allie.Chat.Lib.ViewModels.Bots;
using Allie.Chat.Tcp;
using Allie.Chat.WebAPI;
using System.Threading.Tasks;

namespace Allie.Chat.Commands.Tcp
{
    public class CommandsTcp : BaseCommandsTokenService, ICommandsService
    {
        protected readonly ITcpClientAC _tcpClient;

        protected int _reconnectPollingIntervalMS;
        protected int _reconnectPollingIntervalIndex;
        protected bool _isRunning;

        private const int RECONNECT_POLLING_INTERVAL_MS = 15000;

        public CommandsTcp(IParametersToken parameters)
            : base(parameters)
        {
            _tcpClient = new TcpClientAC(_parameters.BotAccessToken);
            _tcpClient.MessageEvent += OnMessageEvent;

            Task.Run(async () =>
            {
                _isRunning = true;
                await UpdateEventsAsync(0);

                _tcpClient.Connect();
            });
        }

        protected override void UpdateEvents(int updateIntervalMS)
        {
            if (_isRunning && _bot != null)
            {
                _reconnectPollingIntervalIndex += updateIntervalMS;

                if (_reconnectPollingIntervalIndex >= RECONNECT_POLLING_INTERVAL_MS)
                {
                    _reconnectPollingIntervalIndex = 0;

                    if (_tcpClient != null &&
                        !_tcpClient.IsRunning)
                    {
                        _tcpClient.Connect();
                    }
                }
            }

            base.UpdateEvents(updateIntervalMS);
        }
        protected override async Task<BotVM> GetBotAsync()
        {
            return await _webapiClient.GetBotTcpAsync(_parameters.BotAccessToken);
        }

        public override async Task SendMessageAsync(string message)
        {
            await _tcpClient.SendAsync(message);
        }

        public override void Dispose()
        {
            _isRunning = false;

            if (_tcpClient != null)
            {
                _tcpClient.MessageEvent -= OnMessageEvent;
                _tcpClient.Dispose();
            }

            base.Dispose();
        }
    }
}
