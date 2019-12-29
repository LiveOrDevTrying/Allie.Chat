using Allie.Chat.Commands.Core;
using Allie.Chat.Commands.Core.Auth;
using Allie.Chat.Commands.Core.Events;
using Allie.Chat.Commands.Core.Interfaces;
using Allie.Chat.Lib.ViewModels.Bots;
using Allie.Chat.Tcp;
using Allie.Chat.WebAPI.Auth;
using PHS.Core.Events.Args.NetworkEventArgs;
using System;
using System.Threading.Tasks;
using Tcp.NET.Core.Events.Args;

namespace Allie.Chat.Commands.Tcp
{
    public abstract class BaseCommandsTcpAuth : BaseCommandsAuthService, ICommandsService
    {
        protected ITcpClientAC _tcpClient;
        protected int _reconnectPollingIntervalIndex;
        protected bool _isRunning;

        
        public BaseCommandsTcpAuth(IParameters parameters)
            : base(parameters)
        {
            Task.Run(async () =>
            {
                await GetAccessTokenAsync();
            });
        }

        protected abstract Task GetAccessTokenAsync();

        protected virtual void Connect()
        {
            Disconnect();

            _tcpClient = new TcpClientAC(_parameters.BotAccessToken);
            _tcpClient.ConnectionEvent += OnConnectionEvent;
            _tcpClient.MessageEvent += OnMessageEvent;
            _tcpClient.ErrorEvent += OnErrorEvent;
            _tcpClient.Connect();

            _isRunning = true;
        }
        protected virtual void Disconnect()
        {
            _isRunning = false;

            if (_tcpClient != null)
            {
                _tcpClient.ConnectionEvent -= OnConnectionEvent;
                _tcpClient.MessageEvent -= OnMessageEvent;
                _tcpClient.ErrorEvent -= OnErrorEvent;
                _tcpClient.Disconnect();
                _tcpClient.Dispose();
            }
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

        protected virtual Task OnErrorEvent(object sender, TcpErrorEventArgs args)
        {
            FireErrorEvent(sender, args);
            return Task.CompletedTask;
        }
        protected virtual Task OnConnectionEvent(object sender, TcpConnectionEventArgs args)
        {
            FireConnectionEvent(sender, args);
            return Task.CompletedTask;
        }

        public override async Task SendMessageAsync(string message)
        {
            await _tcpClient.SendAsync(message);
        }

        public override void Dispose()
        {
            Disconnect();

            base.Dispose();
        }
    }
}