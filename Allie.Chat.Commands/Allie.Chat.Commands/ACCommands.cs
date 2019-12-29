using Allie.Chat.Commands.Core;
using Allie.Chat.Commands.Core.Events;
using Allie.Chat.Commands.Core.Events.Args;
using Allie.Chat.Commands.Enums;
using Allie.Chat.Commands.Interfaces;
using Allie.Chat.Commands.Tcp;
using Allie.Chat.Commands.Tcp.Auth;
using Allie.Chat.Commands.Websocket;
using Allie.Chat.Commands.Websocket.Auth;
using PHS.Core.Events.Args.NetworkEventArgs;
using System;

namespace Allie.Chat.Commands
{
    public class ACCommands : IDisposable, IACCommands
    {
        protected readonly ICommandsService _service;
        protected readonly IACParameters _parameters;

        public event ClientEventHandler<ConnectionEventArgs> ConnectionEvent;
        public event ClientEventHandler<ErrorEventArgs> ErrorEvent;
        public event CommandEventHandler<CommandDiscordEventArgs> CommandDiscordEvent;
        public event CommandEventHandler<CommandEventArgs> CommandEvent;
        public event CommandEventHandler<CommandTcpEventArgs> CommandTcpEvent;
        public event CommandEventHandler<CommandWSEventArgs> CommandWebsocketEvent;
        public event CommandEventHandler<CommandTwitchEventArgs> CommandTwitchEvent;

        public ACCommands(IACParametersAuthCode parameters)
        {
            _parameters = parameters;

            switch (parameters.ClientType)
            {
                case ClientType.Tcp:
                    _service = new CommandsTcpAuthCode(parameters);
                    break;
                case ClientType.Websocket:
                    _service = new CommandsWebsocketAuthCode(parameters);
                    break;
                default:
                    throw new Exception("ClientType is not accepted");
            }

            _service.ConnectionEvent += ConnectionEvent;
            _service.ErrorEvent += ErrorEvent;
            _service.CommandDiscordEvent += CommandDiscordEvent;
            _service.CommandEvent += CommandEvent;
            _service.CommandTcpEvent += CommandTcpEvent;
            _service.CommandWebsocketEvent += CommandWebsocketEvent;
            _service.CommandTwitchEvent += CommandTwitchEvent;
        }
        public ACCommands(IACParametersAuthPKCE parameters)
        {
            _parameters = parameters;

            switch (parameters.ClientType)
            {
                case ClientType.Tcp:
                    _service = new CommandsTcpAuthPKCE(parameters);
                    break;
                case ClientType.Websocket:
                    _service = new CommandsWebsocketAuthPKCE(parameters);
                    break;
                default:
                    throw new Exception("ClientType is not accepted");
            }

            _service.ConnectionEvent += ConnectionEvent;
            _service.ErrorEvent += ErrorEvent;
            _service.CommandDiscordEvent += CommandDiscordEvent;
            _service.CommandEvent += CommandEvent;
            _service.CommandTcpEvent += CommandTcpEvent;
            _service.CommandWebsocketEvent += CommandWebsocketEvent;
            _service.CommandTwitchEvent += CommandTwitchEvent;
        }
        public ACCommands(IACParametersAuthROPassword parameters)
        {
            _parameters = parameters;

            switch (parameters.ClientType)
            {
                case ClientType.Tcp:
                    _service = new CommandsTcpAuthROPassword(parameters);
                    break;
                case ClientType.Websocket:
                    _service = new CommandsWebsocketAuthROPassword(parameters);
                    break;
                default:
                    throw new Exception("ClientType is not accepted");
            }

            _service.ConnectionEvent += ConnectionEvent;
            _service.ErrorEvent += ErrorEvent;
            _service.CommandDiscordEvent += CommandDiscordEvent;
            _service.CommandEvent += CommandEvent;
            _service.CommandTcpEvent += CommandTcpEvent;
            _service.CommandWebsocketEvent += CommandWebsocketEvent;
            _service.CommandTwitchEvent += CommandTwitchEvent;
        }
        public ACCommands(IACParametersToken parameters)
        {
            _parameters = parameters;

            switch (parameters.ClientType)
            {
                case ClientType.Tcp:
                    _service = new CommandsTcp(parameters);
                    break;
                case ClientType.Websocket:
                    _service = new CommandsWebsocket(parameters);
                    break;
                default:
                    throw new Exception("ClientType is not accepted");
            }

            _service.ConnectionEvent += ConnectionEvent;
            _service.ErrorEvent += ErrorEvent;
            _service.CommandDiscordEvent += CommandDiscordEvent;
            _service.CommandEvent += CommandEvent;
            _service.CommandTcpEvent += CommandTcpEvent;
            _service.CommandWebsocketEvent += CommandWebsocketEvent;
            _service.CommandTwitchEvent += CommandTwitchEvent;
        }

        public virtual void Dispose()
        {
            if (_service != null)
            {
                _service.ConnectionEvent -= ConnectionEvent;
                _service.ErrorEvent -= ErrorEvent;
                _service.CommandDiscordEvent -= CommandDiscordEvent;
                _service.CommandEvent -= CommandEvent;
                _service.CommandTcpEvent -= CommandTcpEvent;
                _service.CommandWebsocketEvent -= CommandWebsocketEvent;
                _service.CommandTwitchEvent -= CommandTwitchEvent;
                _service.Dispose();
            }
        }
    }
}
