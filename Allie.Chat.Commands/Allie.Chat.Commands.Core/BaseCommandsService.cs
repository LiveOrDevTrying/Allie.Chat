using Allie.Chat.Commands.Core.Events;
using Allie.Chat.Commands.Core.Events.Args;
using Allie.Chat.Commands.Core.Interfaces;
using Allie.Chat.Commands.Core.Models;
using Allie.Chat.Commands.Core.Services;
using Allie.Chat.Lib.DTOs.Commands;
using Allie.Chat.Lib.DTOs.Streams;
using Allie.Chat.Lib.Enums;
using Allie.Chat.Lib.Interfaces;
using Allie.Chat.Lib.ViewModels.Bots;
using Allie.Chat.WebAPI;
using PHS.Core.Events.Args.NetworkEventArgs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Allie.Chat.Commands.Core
{
    public abstract class BaseCommandsService : BasePollingService, ICommandsService
    {
        protected readonly IParameters _parameters;
        protected ConcurrentDictionary<Guid, CachedStreamDTO> _cachedData =
            new ConcurrentDictionary<Guid, CachedStreamDTO>();
        protected BotVM _bot;
        protected IWebAPIClientAC _webapiClient;
        
        private event CommandEventHandler<CommandEventArgs> _commandEvent;
        private event CommandEventHandler<CommandTwitchEventArgs> _commandTwitchEvent;
        private event CommandEventHandler<CommandDiscordEventArgs> _commandDiscordEvent;
        private event CommandEventHandler<CommandTcpEventArgs> _commandTcpEvent;
        private event CommandEventHandler<CommandWSEventArgs> _commandWSEvent;
        private event ClientEventHandler<ConnectionEventArgs> _connectionEvent;
        private event ClientEventHandler<ErrorEventArgs> _errorEvent;

        public BaseCommandsService(IParameters parameters)
            : base(parameters.StreamCachePollingIntervalMS)
        {
            _parameters = parameters;
        }

        public abstract Task SendMessageAsync(string message);

        protected virtual void OnMessageEvent(object sender, IMessageBase message)
        {
            if (_cachedData.TryGetValue(message.Stream.Id, out var cachedStream))
            {
                foreach (var cachedCommandSet in cachedStream.CommandSets)
                {
                    if (message.Message.Trim().ToLower().StartsWith(cachedCommandSet.CommandSet.Prefix))
                    {
                        var commandSubstring = message.Message.Substring(cachedCommandSet.CommandSet.Prefix.Length);

                        foreach (var command in cachedCommandSet.Commands)
                        {
                            if (command.CommandText.Trim().ToLower().StartsWith(commandSubstring.Trim().ToLower()))
                            {
                                // This is a valid command, fire the event
                                switch (message)
                                {
                                    case IMessageTwitch c:
                                        FireCommandTwitchReceivedEvent(sender, new CommandTwitchEventArgs
                                        {
                                            Command = command,
                                            CommandSet = cachedCommandSet.CommandSet,
                                            Message = c
                                        });
                                        break;
                                    case IMessageDiscord c:
                                        FireCommandDiscordReceivedEvent(sender, new CommandDiscordEventArgs
                                        {
                                            Command = command,
                                            CommandSet = cachedCommandSet.CommandSet,
                                            Message = c
                                        });
                                        break;
                                    case IMessageTcp c:
                                        FireCommandTcpReceivedEvent(sender, new CommandTcpEventArgs
                                        {
                                            Command = command,
                                            CommandSet = cachedCommandSet.CommandSet,
                                            Message = c
                                        });
                                        break;
                                    case IMessageWS c:
                                        FireCommandWSReceivedEvent(sender, new CommandWSEventArgs
                                        {
                                            Command = command,
                                            CommandSet = cachedCommandSet.CommandSet,
                                            Message = c
                                        });
                                        break;
                                    default:
                                        break;
                                }

                                FireCommandReceivedEvent(sender, new CommandEventArgs
                                {
                                    Command = command,
                                    CommandSet = cachedCommandSet.CommandSet,
                                    Message = message
                                });
                            }
                        }
                    }
                }
            }
        }

        protected override void UpdateEvents(int updateIntervalMS)
        { }
        protected override async Task UpdateEventsAsync(int updateIntervalMS)
        {
            if (_bot == null)
            {
                _bot = await GetBotAsync();

                if (_bot == null)
                {
                    return;
                }
            }

            var cachedStreams = new Dictionary<Guid, CachedStreamDTO>();

            var streams = await _webapiClient.GetStreamsAsync();

            foreach (var stream in streams)
            {
                if (!cachedStreams.ContainsKey(stream.Id))
                {
                    var routes = await _webapiClient.GetRoutesAsync(stream.Id);

                    foreach (var route in routes)
                    {
                        switch (route.RouteType)
                        {
                            case RouteType.Inbound:
                                // Now check if the route belongs to the bot
                                var routeVM = await _webapiClient.GetRouteAsync(route.Id);

                                if (routeVM.Bot.Id == _bot.Id)
                                {
                                    // This route is registered to the bot

                                    // Verify we have at least 1 path registered
                                    var paths = await _webapiClient.GetPathsAsync(route.Id);

                                    if (paths.Any())
                                    {
                                        // Create your cached data
                                        var cachedStream = await CreateCachedStreamAsync(stream);
                                        cachedStreams.TryAdd(stream.Id, cachedStream);
                                    }
                                }
                                break;
                            case RouteType.Outbound:
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            _cachedData = new ConcurrentDictionary<Guid, CachedStreamDTO>(cachedStreams);
        }
        protected virtual async Task<CachedStreamDTO> CreateCachedStreamAsync(StreamDTO stream)
        {
            var cachedStreamDTO = new CachedStreamDTO
            {
                Stream = stream
            };

            var cachedCommandSets = new List<CachedCommandSetDTO>();
            var streamCommandSets = await _webapiClient.GetStreamCommandSetsAsync(stream.Id);

            foreach (var streamCommandSet in streamCommandSets)
            {
                if (streamCommandSet != null &&
                    streamCommandSet.CommandSet != null)
                {
                    var commands = await _webapiClient.GetCommandsAsync(streamCommandSet.CommandSet.Id);

                    cachedCommandSets.Add(new CachedCommandSetDTO
                    {
                        CommandSet = new CommandSetDTO
                        {
                            CommandsCount = commands.Count(),
                            Description = streamCommandSet.CommandSet.Description,
                            Id = streamCommandSet.CommandSet.Id,
                            Name = streamCommandSet.CommandSet.Name,
                            Prefix = streamCommandSet.CommandSet.Prefix,
                            StreamsCommandSets = streamCommandSets.Count()
                        },
                        Commands = commands
                    });
                }

                cachedStreamDTO.CommandSets = cachedCommandSets.ToArray();
            }

            return cachedStreamDTO;
        }
        protected abstract Task<BotVM> GetBotAsync();

        public virtual void UpdateWebAPIToken(string webAPIToken)
        {
            if (_webapiClient != null)
            {
                _webapiClient.Dispose();
            }

            _webapiClient = new WebAPIClientAC(webAPIToken);
        }

        protected virtual void FireCommandReceivedEvent(object sender, CommandEventArgs args)
        {
            _commandEvent?.Invoke(sender, args);
        }
        protected virtual void FireCommandTwitchReceivedEvent(object sender, CommandTwitchEventArgs args)
        {
            _commandTwitchEvent?.Invoke(sender, args);
        }
        protected virtual void FireCommandDiscordReceivedEvent(object sender, CommandDiscordEventArgs args)
        {
            _commandDiscordEvent?.Invoke(sender, args);
        }
        protected virtual void FireCommandTcpReceivedEvent(object sender, CommandTcpEventArgs args)
        {
            _commandTcpEvent?.Invoke(sender, args);
        }
        protected virtual void FireCommandWSReceivedEvent(object sender, CommandWSEventArgs args)
        {
            _commandWSEvent?.Invoke(sender, args);
        }
        protected virtual void FireConnectionEvent(object sender, ConnectionEventArgs args)
        {
            _connectionEvent?.Invoke(sender, args);
        }
        protected virtual void FireErrorEvent(object sender, ErrorEventArgs args)
        {
            _errorEvent?.Invoke(sender, args);
        }

        public override void Dispose()
        {
            if (_webapiClient != null)
            {
                _webapiClient.Dispose();
            }

            base.Dispose();
        }

        public event CommandEventHandler<CommandEventArgs> CommandEvent
        {
            add
            {
                _commandEvent += value;
            }
            remove
            {
                _commandEvent -= value;
            }
        }
        public event CommandEventHandler<CommandTwitchEventArgs> CommandTwitchEvent
        {
            add
            {
                _commandTwitchEvent += value;
            }
            remove
            {
                _commandTwitchEvent -= value;
            }
        }
        public event CommandEventHandler<CommandDiscordEventArgs> CommandDiscordEvent
        {
            add
            {
                _commandDiscordEvent += value;
            }
            remove
            {
                _commandDiscordEvent -= value;
            }
        }
        public event CommandEventHandler<CommandTcpEventArgs> CommandTcpEvent
        {
            add
            {
                _commandTcpEvent += value;
            }
            remove
            {
                _commandTcpEvent -= value;
            }
        }
        public event CommandEventHandler<CommandWSEventArgs> CommandWebsocketEvent
        {
            add
            {
                _commandWSEvent += value;
            }
            remove
            {
                _commandWSEvent -= value;
            }
        }
        public event ClientEventHandler<ConnectionEventArgs> ConnectionEvent
        {
            add
            {
                _connectionEvent += value;
            }
            remove
            {
                _connectionEvent -= value;
            }
        }
        public event ClientEventHandler<ErrorEventArgs> ErrorEvent
        {
            add
            {
                _errorEvent += value;
            }
            remove
            {
                _errorEvent -= value;
            }
        }
    }
}
