using Allie.Chat.Commands.Core;
using Allie.Chat.Commands.Tcp.Interfaces;
using Allie.Chat.Lib.ViewModels.Bots;
using Allie.Chat.Tcp;
using Allie.Chat.WebAPI;
using System;
using System.Threading.Tasks;

namespace Allie.Chat.Commands.Tcp
{
    public class CommandsTcp : BaseCommandsService, ICommandsService
    {
        protected readonly ITcpClientAC _tcpClient;
        protected readonly IParametersCommandsTcp _parameters;

        protected int _reconnectPollingIntervalMS;
        protected int _reconnectPollingIntervalIndex;
        protected bool _isRunning;

        public CommandsTcp(IParametersCommandsTcp parameters)
            : base(parameters.WebAPIToken, parameters.StreamCachePollingIntervalMS, new WebAPIClientAC(parameters.WebAPIToken))
        {
            _parameters = parameters;

            _tcpClient = new TcpClientAC(_parameters.BotAccessToken);
            _tcpClient.MessageEvent += OnMessageEvent;
            _tcpClient.Connect();

            _isRunning = true;
        }

        protected override void UpdateEvents(int updateIntervalMS)
        {
            if (_isRunning && _bot != null)
            {
                _reconnectPollingIntervalIndex += updateIntervalMS;

                if (_reconnectPollingIntervalIndex >= _parameters.ReconnectPollingIntervalMS)
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
