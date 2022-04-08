using Allie.Chat.Auth.Interfaces;
using Allie.Chat.Events.Args;
using Allie.Chat.Interfaces;
using Allie.Chat.Models;
using Allie.Chat.Lib.DTOs.Streams;
using Allie.Chat.Lib.Enums;
using Allie.Chat.Lib.Interfaces;
using Allie.Chat.Lib.ViewModels.Bots;
using Allie.Chat.WebAPI;
using IdentityModel.Client;
using IdentityModel.OidcClient;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Allie.Chat.Lib.DTOs.Commands;

namespace Allie.Chat.Services
{
    public class CommandsService : BasePollingService, ICommandsService
    {
        protected readonly IParameters _parameters;
        protected ConcurrentDictionary<Guid, CachedStreamDTO> _cachedData =
            new ConcurrentDictionary<Guid, CachedStreamDTO>();
        protected BotVM _bot;
        protected LoginResult _loginResult;
        protected IWebAPIClientAC _webAPIClient;
        protected TokenResponse _tokenResponse;
        protected DateTime _expireTime;

        private const int STREAM_CACHE_POLLING_INTERVAL_MS = 45000;

        public CommandsService(IParametersToken parameters)
            : base(STREAM_CACHE_POLLING_INTERVAL_MS)
        {
            _parameters = parameters;

            UpdateWebAPIToken(parameters.WebAPIToken);
        }
        public CommandsService(IParametersAuthCode parameters)
            : base(STREAM_CACHE_POLLING_INTERVAL_MS)
        {
            _parameters = parameters;

            Task.Run(async () =>
            {
                await GetAccessTokenAsync();
            });
        }
        public CommandsService(IParametersAuthPKCE parameters)
            : base(STREAM_CACHE_POLLING_INTERVAL_MS)
        {
            _parameters = parameters;

            Task.Run(async () =>
            {
                await GetAccessTokenAsync();
            });
        }
        public CommandsService(IParametersAuthROPassword parameters)
            : base(STREAM_CACHE_POLLING_INTERVAL_MS)
        {
            _parameters = parameters;

            Task.Run(async () =>
            {
                await GetAccessTokenAsync();
            });
        }

        public virtual CommandEventArgs[] ProcessMessage(IMessageBase message)
        {
            var args = new List<CommandEventArgs>();

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
                                        args.Add(new CommandTwitchEventArgs
                                        {
                                            Command = command,
                                            CommandSet = cachedCommandSet.CommandSet,
                                            Message = c
                                        });
                                        break;
                                    case IMessageDiscord c:
                                        args.Add(new CommandDiscordEventArgs
                                        {
                                            Command = command,
                                            CommandSet = cachedCommandSet.CommandSet,
                                            Message = c
                                        });
                                        break;
                                    case IMessageTcp c:
                                        args.Add(new CommandTcpEventArgs
                                        {
                                            Command = command,
                                            CommandSet = cachedCommandSet.CommandSet,
                                            Message = c
                                        });
                                        break;
                                    case IMessageWS c:
                                        args.Add( new CommandWSEventArgs
                                        {
                                            Command = command,
                                            CommandSet = cachedCommandSet.CommandSet,
                                            Message = c
                                        });
                                        break;
                                    default:
                                        break;
                                }

                                args.Add(new CommandEventArgs
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

            return args.ToArray();
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

            var streams = await _webAPIClient.GetStreamsAsync();

            foreach (var stream in streams)
            {
                if (!cachedStreams.ContainsKey(stream.Id))
                {
                    var routes = await _webAPIClient.GetRoutesAsync(stream.Id);

                    foreach (var route in routes)
                    {
                        switch (route.RouteType)
                        {
                            case RouteType.Inbound:
                                // Now check if the route belongs to the bot
                                var routeVM = await _webAPIClient.GetRouteAsync(route.Id);

                                if (routeVM.Bot.Id == _bot.Id)
                                {
                                    // This route is registered to the bot

                                    // Verify we have at least 1 path registered
                                    var paths = await _webAPIClient.GetPathsAsync(route.Id);

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
            var streamCommandSets = await _webAPIClient.GetStreamCommandSetsAsync(stream.Id);

            foreach (var streamCommandSet in streamCommandSets)
            {
                if (streamCommandSet != null &&
                    streamCommandSet.CommandSet != null)
                {
                    var commands = await _webAPIClient.GetCommandsAsync(streamCommandSet.CommandSet.Id);

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
        protected virtual async Task<BotVM> GetBotAsync()
        {
            return await _webAPIClient.GetBotAsync(_parameters.BotAccessToken);
        }
        protected virtual async Task GetAccessTokenAsync()
        {
            if (_webAPIClient == null)
            {
                _webAPIClient = new WebAPIClientAC();
            }
            switch (_parameters)
            {
                case IParametersAuthCode c:
                    _loginResult = await _webAPIClient.GetAccessTokenAuthCodeAsync(c.ClientId, c.ClientSecret, c.Scopes);

                    if (_loginResult != null)
                    {
                        UpdateWebAPIToken(_loginResult.AccessToken);
                        await UpdateEventsAsync(0);
                    }
                    break;
                case IParametersAuthPKCE c:
                    _loginResult = await _webAPIClient.GetAccessTokenNativePKCEAsync(c.ClientId, c.Scopes);

                    if (_loginResult != null)
                    {
                        if (!string.IsNullOrWhiteSpace(_loginResult.AccessToken))
                        {
                            UpdateWebAPIToken(_loginResult.AccessToken);
                            await UpdateEventsAsync(0);
                        }
                    }
                    break;
                case IParametersAuthROPassword c:
                    _tokenResponse = await _webAPIClient.GetAccessTokenResourceOwnerPasswordAsync(
                        c.ClientId, c.ClientSecret, c.Scopes, c.Username, c.Password);

                    if (_tokenResponse != null)
                    {
                        if (!string.IsNullOrWhiteSpace(_tokenResponse.AccessToken))
                        {
                            _expireTime = DateTime.UtcNow + TimeSpan.FromSeconds(_tokenResponse.ExpiresIn * 0.8f);
                            UpdateWebAPIToken(_tokenResponse.AccessToken);
                            await UpdateEventsAsync(0);
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        protected virtual async Task ValidateTokenAsync()
        {
            switch (_parameters)
            {
                case IParametersAuthCode c:
                case IParametersAuthPKCE d:
                    if (_loginResult.AccessTokenExpiration <= DateTime.Now)
                    {
                        await GetAccessTokenAsync();
                    }
                    break;
                case IParametersAuthROPassword c:
                    if (_expireTime <= DateTime.UtcNow)
                    {
                        await GetAccessTokenAsync();
                    }
                    break;
                default:
                    break;
            }
        }
        public virtual void UpdateWebAPIToken(string webAPIToken)
        {
            if (_webAPIClient != null)
            {
                _webAPIClient.Dispose();
            }

            _webAPIClient = new WebAPIClientAC(webAPIToken);
        }

        public override void Dispose()
        {
            if (_webAPIClient != null)
            {
                _webAPIClient.Dispose();
            }

            base.Dispose();
        }
    }
}
