using Allie.Chat.Services;
using Allie.Chat.Events;
using Allie.Chat.Events.Args;
using Allie.Chat.Enums;
using Allie.Chat.Interfaces;
using Allie.Chat.Lib.Interfaces;
using Allie.Chat.Tcp;
using Allie.Chat.Websocket;
using PHS.Networking.Events.Args;
using System;
using System.Threading.Tasks;
using Tcp.NET.Client.Events.Args;
using WebsocketsSimple.Client.Events.Args;
using Allie.Chat.WebAPI;

namespace Allie.Chat
{
    public class ACCommands : IACCommands
    {
        protected readonly ICommandsService _service;
        protected readonly IACParameters _parameters;
        protected readonly ITcpClientAC _tcpClient;
        protected readonly IWSClientAC _wsClient;

        public event ClientEventHandler<ConnectionEventArgs> ConnectionEvent;
        public event ClientEventHandler<ErrorEventArgs> ErrorEvent;
        public event CommandEventHandler<CommandDiscordEventArgs> CommandDiscordEvent;
        public event CommandEventHandler<CommandEventArgs> CommandEvent;
        public event CommandEventHandler<CommandTcpEventArgs> CommandTcpEvent;
        public event CommandEventHandler<CommandWSEventArgs> CommandWebsocketEvent;
        public event CommandEventHandler<CommandTwitchEventArgs> CommandTwitchEvent;

        public ACCommands(IACParametersToken parameters)
        {
            _parameters = parameters;
            _service = new CommandsService(parameters, new WebAPIClientAC());

            switch (parameters.ClientType)
            {
                case ClientType.Tcp:
                    _tcpClient = new TcpClientAC(_parameters.BotAccessToken);
                    _tcpClient.ConnectionEvent += OnTcpConnectionEvent;
                    _tcpClient.ErrorEvent += OnTcpErrorEvent;
                    _tcpClient.MessageEvent += OnMessageEvent;
                    break;
                case ClientType.Websocket:
                    _wsClient = new WSClientAC(_parameters.BotAccessToken);
                    _wsClient.ConnectionEvent += OnWSConnectionEvent;
                    _wsClient.ErrorEvent += OnWSErrorEvent;
                    _wsClient.MessageEvent += OnMessageEvent;
                    break;
                default:
                    throw new Exception("ClientType is not accepted");
            }
        }
        public ACCommands(IACParametersAuthCode parameters)
        {
            _parameters = parameters;

            _service = new CommandsService(parameters, new WebAPIClientAC());
            switch (parameters.ClientType)
            {
                case ClientType.Tcp:
                    _tcpClient = new TcpClientAC(_parameters.BotAccessToken);
                    _tcpClient.ConnectionEvent += OnTcpConnectionEvent;
                    _tcpClient.ErrorEvent += OnTcpErrorEvent;
                    _tcpClient.MessageEvent += OnMessageEvent;
                    break;
                case ClientType.Websocket:
                    _wsClient = new WSClientAC(_parameters.BotAccessToken);
                    _wsClient.ConnectionEvent += OnWSConnectionEvent;
                    _wsClient.ErrorEvent += OnWSErrorEvent;
                    _wsClient.MessageEvent += OnMessageEvent;
                    break;
                default:
                    throw new Exception("ClientType is not accepted");
            }
        }
        public ACCommands(IACParametersAuthPKCE parameters)
        {
            _parameters = parameters;

            _service = new CommandsService(parameters, new WebAPIClientAC());
            switch (parameters.ClientType)
            {
                case ClientType.Tcp:
                    _tcpClient = new TcpClientAC(_parameters.BotAccessToken);
                    _tcpClient.ConnectionEvent += OnTcpConnectionEvent;
                    _tcpClient.ErrorEvent += OnTcpErrorEvent;
                    _tcpClient.MessageEvent += OnMessageEvent;
                    break;
                case ClientType.Websocket:
                    _wsClient = new WSClientAC(_parameters.BotAccessToken);
                    _wsClient.ConnectionEvent += OnWSConnectionEvent;
                    _wsClient.ErrorEvent += OnWSErrorEvent;
                    _wsClient.MessageEvent += OnMessageEvent;
                    break;
                default:
                    throw new Exception("ClientType is not accepted");
            }
        }
        public ACCommands(IACParametersAuthROPassword parameters)
        {
            _parameters = parameters;

            _service = new CommandsService(parameters, new WebAPIClientAC());
            switch (parameters.ClientType)
            {
                case ClientType.Tcp:
                    _tcpClient = new TcpClientAC(_parameters.BotAccessToken);
                    _tcpClient.ConnectionEvent += OnTcpConnectionEvent;
                    _tcpClient.ErrorEvent += OnTcpErrorEvent;
                    _tcpClient.MessageEvent += OnMessageEvent;
                    break;
                case ClientType.Websocket:
                    _wsClient = new WSClientAC(_parameters.BotAccessToken);
                    _wsClient.ConnectionEvent += OnWSConnectionEvent;
                    _wsClient.ErrorEvent += OnWSErrorEvent;
                    _wsClient.MessageEvent += OnMessageEvent;
                    break;
                default:
                    throw new Exception("ClientType is not accepted");
            }
        }

        public virtual async Task StartAsync()
        {
            if (_tcpClient != null)
            {
                await _tcpClient.ConnectAsync();
            }

            if (_wsClient != null)
            {
                await _wsClient.ConnectAsync();
            }
        }
        public virtual void Update(int updateIntervalMS)
        {
            _service.Update(updateIntervalMS);
        }
        protected virtual void OnMessageEvent(object sender, IMessageBase args)
        {
            var responses = _service.ProcessMessage(args);

            foreach (var response in responses)
            {
                switch (response)
                {
                    case CommandTwitchEventArgs c:
                        CommandTwitchEvent?.Invoke(sender, c);
                        break;
                    case CommandDiscordEventArgs c:
                        CommandDiscordEvent?.Invoke(sender, c);
                        break;
                    case CommandWSEventArgs c:
                        CommandWebsocketEvent?.Invoke(sender, c);
                        break;
                    case CommandTcpEventArgs c:
                        CommandTcpEvent?.Invoke(sender, c);
                        break;
                    default:
                        break;
                }

                CommandEvent?.Invoke(sender, response);
            }
        }
        protected virtual void OnTcpErrorEvent(object sender, TcpErrorClientEventArgs args)
        {
            ErrorEvent?.Invoke(sender, args);
        }
        protected virtual void OnTcpConnectionEvent(object sender, TcpConnectionClientEventArgs args)
        {
            ConnectionEvent?.Invoke(sender, args);
        }
        protected virtual void OnWSErrorEvent(object sender, WSErrorClientEventArgs args)
        {
            ErrorEvent?.Invoke(sender, args);
        }
        protected virtual void OnWSConnectionEvent(object sender, WSConnectionClientEventArgs args)
        {
            ConnectionEvent?.Invoke(sender, args);
        }

        public virtual void Dispose()
        {
            if (_tcpClient != null) 
            {
                _tcpClient.Disconnect();
                _tcpClient.ConnectionEvent -= OnTcpConnectionEvent;
                _tcpClient.ErrorEvent -= OnTcpErrorEvent;
                _tcpClient.MessageEvent -= OnMessageEvent;
                _tcpClient.Dispose();
            }

            if (_wsClient != null)
            {
                _wsClient.DisconnectAsync().Wait();
                _wsClient.ConnectionEvent -= OnWSConnectionEvent;
                _wsClient.ErrorEvent -= OnWSErrorEvent;
                _wsClient.MessageEvent -= OnMessageEvent;
                _wsClient.Dispose();
            }

            if (_service != null)
            {
                _service.Dispose();
            }
        }
    }
}
