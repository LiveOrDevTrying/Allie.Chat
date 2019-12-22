using Allie.Chat.Lib.DTOs;
using Allie.Chat.Lib.DTOs.Bots;
using Allie.Chat.Lib.DTOs.ClientApplications;
using Allie.Chat.Lib.DTOs.Commands;
using Allie.Chat.Lib.DTOs.Currencies;
using Allie.Chat.Lib.DTOs.Paths;
using Allie.Chat.Lib.DTOs.Routes;
using Allie.Chat.Lib.DTOs.StreamsCurrencies;
using Allie.Chat.Lib.DTOs.Streams;
using Allie.Chat.Lib.Requests.Bots;
using Allie.Chat.Lib.Requests.ClientApplications;
using Allie.Chat.Lib.Requests.Commands;
using Allie.Chat.Lib.Requests.Currencies;
using Allie.Chat.Lib.Requests.Paths;
using Allie.Chat.Lib.Requests.Routes;
using Allie.Chat.Lib.Requests.StreamsCurrencies;
using Allie.Chat.Lib.Requests.Streams;
using Allie.Chat.Lib.Responses.Servers;
using Allie.Chat.Lib.Responses.Users;
using Allie.Chat.Lib.ViewModels;
using Allie.Chat.Lib.ViewModels.ApplicationUsers;
using Allie.Chat.Lib.ViewModels.Bots;
using Allie.Chat.Lib.ViewModels.ClientApplications;
using Allie.Chat.Lib.ViewModels.Commands;
using Allie.Chat.Lib.ViewModels.Currencies;
using Allie.Chat.Lib.ViewModels.Paths;
using Allie.Chat.Lib.ViewModels.Routes;
using Allie.Chat.Lib.ViewModels.Servers;
using Allie.Chat.Lib.ViewModels.StreamsCurrencies;
using Allie.Chat.Lib.ViewModels.Streams;
using Allie.Chat.Lib.ViewModels.Users;
using System;
using System.Threading.Tasks;
using Allie.Chat.Lib.DTOs.Servers.Channels;
using Allie.Chat.Lib.ViewModels.Servers.Channels;
using Allie.Chat.Lib.DTOs.Users;
using Allie.Chat.Lib.Responses.Currencies;

namespace Allie.Chat.WebAPI
{
    public interface IWebAPIClientAC : IDisposable
    {
        /// <summary>
        /// Set the access token
        /// </summary>
        /// <param name="accessToken"></param>
        void SetAccessToken(string accessToken);

        /// <summary>
        /// Get the registered Api Resources
        /// </summary>
        /// <returns>An array of Api Resource data-transfer objects</returns>
        Task<ApiResourceDTO[]> GetApiResourcesAsync();
        /// <summary>
        /// Get the registered Api Resource
        /// </summary>
        /// <param name="id">The Id of the Api Resource to retrieve</param>
        /// <returns>An Api Resource ViewModel</returns>
        Task<ApiResourceVM> GetApiResourceAsync(int id);
        /// <summary>
        /// Create an Api Resource
        /// </summary>
        /// <param name="request">The Api Resource create request</param>
        /// <returns>The created Api Resource ViewModel</returns>
        Task<ApiResourceVM> CreateApiResourceAsync(ApiResourceCreateRequest request);
        /// <summary>
        /// Update an Api Resource
        /// </summary>
        /// <param name="request"The Api Resource update request></param>
        /// <returns>The updated Api Resource ViewModel</returns>
        Task<ApiResourceVM> UpdateApiResourceAsync(ApiResourceUpdateRequest request);
        /// <summary>
        /// Delete an Api Resource
        /// </summary>
        /// <param name="id">Id of the requested Api Resource to delete</param>
        /// <returns>True if the delete was successful</returns>
        Task<bool> DeleteApiResourceAsync(int id);
        /// <summary>
        /// Reset an Api Resource's Secrets
        /// </summary>
        /// <param name="id">The Id of the Api Resource to reset its secrets</param>
        /// <returns>True if the Api Resource Secrets were successfully reset</returns>
        Task<bool> ResetApiResourceSecretsAsync(int id);
        
        /// <summary>
        /// Retrieve the Authorized Application User
        /// </summary>
        /// <returns>An Application Udser ViewModel</returns>
        Task<ApplicationUserVM> GetApplicationUserAsync();

        /// <summary>
        /// Get the registered Bots
        /// </summary>
        /// <returns>An array of Bot data-transfer objects registered to the Application User</returns>
        Task<BotDTO[]> GetBotsAsync();
        /// <summary>
        /// Get a registered Twitch Bot
        /// </summary>
        /// <param name="id">Id of the requested Twitch Bot</param>
        /// <returns>The Twitch Bot ViewModel</returns>
        Task<BotTwitchVM> GetBotTwitchAsync(Guid id);
        /// <summary>
        /// Get a registered Discord Bot
        /// </summary>
        /// <param name="id">The Id of the Discord Bot</param>
        /// <returns>A Discord Bot ViewModel</returns>
        Task<BotDiscordVM> GetBotDiscordAsync(Guid id);
        /// <summary>
        /// Get a registered Tcp Bot by OAuth Token
        /// </summary>
        /// <param name="token">The OAuth Token of the requested Tcp Bot</param>
        /// <returns>The Tcp Bot ViewModel</returns>
        Task<BotTcpVM> GetBotTcpAsync(string token);
        /// <summary>
        /// Create a Tcp Bot
        /// </summary>
        /// <param name="request">The Tcp Bot create request</param>
        /// <returns>A Tcp Bot ViewModel</returns>
        Task<BotTcpVM> PostBotTcpAsync(BotTcpCreateRequest request);
        /// <summary>
        /// Update a Tcp Bot
        /// </summary>
        /// <param name="request">The Tcp Bot update request</param>
        /// <returns>A Tcp Bot ViewModel</returns>
        Task<BotTcpVM> PutBotTcpAsync(BotTcpUpdateRequest request);
        /// <summary>
        /// Get a Websocket Bot
        /// </summary>
        /// <param name="id">The Id of the requested Websocket Bot</param>
        /// <returns>A Websocket Bot ViewModel</returns>
        Task<BotWSVM> GetBotWebsocketAsync(Guid id);
        /// <summary>
        /// Get a Websocket Bot by OAuth Token
        /// </summary>
        /// <param name="token">The OAuth Token of the requested Websocket Bot</param>
        /// <returns>A Websocket Bot ViewModel</returns>
        Task<BotWSVM> GetBotWebsocketAsync(string token);
        /// <summary>
        /// Create a Websocket Bot
        /// </summary>
        /// <param name="request">A Websocket Bot create request</param>
        /// <returns>A Websocket Bot ViewModel</returns>
        Task<BotWSVM> CreateBotWebsocketAsync(BotWSCreateRequest request);
        /// <summary>
        /// Update a Websocket Bot
        /// </summary>
        /// <param name="request">The Websocket Bot update request</param>
        /// <returns>A Websocket Bot ViewModel</returns>
        Task<BotWSVM> UpdateBotWebsocketAsync(BotWSUpdateRequest request);
        /// <summary>
        /// Delete a Websocket Bot
        /// </summary>
        /// <param name="request">The Websocket Bot update request</param>
        /// <returns>A Websocket Bot ViewModel</returns>
        Task<bool> DeleteBotAsync(Guid id);

        /// <summary>
        /// Get the registered Client Applications
        /// </summary>
        /// <returns>An array of Client Application data-transfer objects</returns>
        Task<ClientApplicationDTO[]> GetClientApplicationsAsync();
        /// <summary>
        /// Get an Authorization Code Client Application
        /// </summary>
        /// <param name="id">The Id of the requested Authorization Code Client Application</param>
        /// <returns>An Authorization Code Client Application ViewModel</returns>
        Task<ClientApplicationAuthCodeVM> GetClientApplicationAuthCodeAsync(int id);
        /// <summary>
        /// Create an Authorization Code Client Application
        /// </summary>
        /// <param name="request">The Authorization Code Client Application create request</param>
        /// <returns>An Authorization Code Client Application ViewModel</returns>
        Task<ClientApplicationAuthCodeVM> CreateClientApplicationAuthCodeAsync(ClientApplicationAuthCodeCreateRequest request);
        /// <summary>
        /// Update an Authorization Code Client Application
        /// </summary>
        /// <param name="request">The Authorization Code Client Application update request</param>
        /// <returns>An Authorization Code Client Application ViewModel</returns>
        Task<ClientApplicationAuthCodeVM> UpdateClientApplicationAuthCodeAsync(ClientApplicationAuthCodeUpdateRequest request);
        /// <summary>
        /// Get an Implicit Client Application
        /// </summary>
        /// <param name="id">The Id of the requested Implicit Client Application</param>
        /// <returns>An Implicit Client Application ViewModel</returns>
        Task<ClientApplicationImplicitVM> GetClientApplicationImplicitAsync(int id);
        /// <summary>
        /// Create an Implicit Client Application
        /// </summary>
        /// <param name="request">The Implicit Client create request</param>
        /// <returns>An Implicit Client Application ViewModel</returns>
        Task<ClientApplicationImplicitVM> CreateClientApplicationImplicitAsync(ClientApplicationImplicitCreateRequest request);
        /// <summary>
        /// Update an Implicit Client Application
        /// </summary>
        /// <param name="request">The Implicit Client update request</param>
        /// <returns>An Implicit Client Application ViewModel</returns>
        Task<ClientApplicationImplicitVM> UpdateClientApplicationImplicitAsync(ClientApplicationImplicitUpdateRequest request);
        /// <summary>
        /// Get a Resource Owner Password Credentials Client Application
        /// </summary>
        /// <param name="id">The Id of the requested Resource Owner Password Credentials Client Application</param>
        /// <returns>A Resource Owner Password Credentials Client Application ViewModel</returns>
        Task<ClientApplicationROPasswordVM> GetClientApplicationPasswordAsync(int id);
        /// <summary>
        /// Create a Resource Owner Password Credentials Client Application
        /// </summary>
        /// <param name="request">A Resource Owner Password Credentials Client Application create request</param>
        /// <returns>A Resource Owner Password Credentials Client Application ViewModel</returns>
        Task<ClientApplicationROPasswordVM> CreateClientApplicationPasswordAsync(ClientApplicationROPasswordCreateRequest request);
        /// <summary>
        /// Update a Resource Owner Password Credentials Client Application
        /// </summary>
        /// <param name="request">A Resource Owner Password Credentials Client Application update request</param>
        /// <returns>A Resource Owner Password Credentials Client Application ViewModel</returns>
        Task<ClientApplicationROPasswordVM> UpdateClientApplicationPasswordAsync(ClientApplicationROPasswordUpdateRequest request);
        /// <summary>
        /// Get a Native PKCE / Authorization Code Client Application
        /// </summary>
        /// <param name="id">The Id of the requested Native PKCE / Authorization Code Client Application</param>
        /// <returns>A Native PKCE / Authorization Code Client APplication ViewModel</returns>
        Task<ClientApplicationPKCEVM> GetClientApplicationNativePKCEAsync(int id);
        /// <summary>
        /// Create a Native PKCE / Authorization Code Client Application
        /// </summary>
        /// <param name="request">The Native PKCE / Authorization Code Client Application create request</param>
        /// <returns>The Native PKCE / Authorization Code Client Application ViewModel</returns>
        Task<ClientApplicationPKCEVM> CreateClientApplicationNativePKCEAsync(ClientApplicationPKCECreateRequest request);
        /// <summary>
        /// Update a Native PKCE / Authorization Code Client Application
        /// </summary>
        /// <param name="request">The Native PKCE / Authorization Code Client Application update request</param>
        /// <returns>The Native PKCE / Authorization Code Client Application ViewModel</returns>
        Task<ClientApplicationPKCEVM> UpdateClientApplicationNativePKCEAsync(ClientApplicationPKCEUpdateRequest request);
        /// <summary>
        /// Delete a Client Application
        /// </summary>
        /// <param name="id">The Id of the Client Application to delete</param>
        /// <returns>A Native PKCE / Authorization Code Client Application ViewModel</returns>
        Task<bool> DeleteClientApplicationAsync(int id);
        /// Reset a Client Application's secrets
        /// </summary>
        /// <param name="id">The Id of the Client Application to reset its secrets</param>
        /// <returns>True if the Client Application's secrets were reset</returns>
        Task<bool> ResetClientApplicationSecretsAsync(int id);

        /// <summary>
        /// Get the registered Command Sets
        /// </summary>
        /// <returns>An array of Command Set data-transfer objects</returns>
        Task<CommandSetDTO[]> GetCommandSetsAsync();
        /// <summary>
        /// Get a Command Set
        /// </summary>
        /// <param name="id">The Id of the requested Command Set</param>
        /// <returns>A Command Set ViewModel</returns>
        Task<CommandSetVM> GetCommandSetAsync(Guid id);
        /// <summary>
        /// Create a Command Set
        /// </summary>
        /// <param name="request">The Command Set create request</param>
        /// <returns>A Command Set ViewModel</returns>
        Task<CommandSetVM> CreateCommandSetAsync(CommandSetCreateRequest request);
        /// <summary>
        /// Update a Command Set
        /// </summary>
        /// <param name="request">The Command Set update request</param>
        /// <returns>A Command Set ViewModel</returns>
        Task<CommandSetVM> UpdateCommandSetAsync(CommandSetUpdateRequest request);
        /// <summary>
        /// Delete a Command Set
        /// </summary>
        /// <param name="id">The Id of the Command Set to be deleted</param>
        /// <returns>True if the delete was successful</returns>
        Task<bool> DeleteCommandSetAsync(Guid id);

        /// <summary>
        /// Get the Commands registered to a Command Set
        /// </summary>
        /// <param name="commandSetId">The Id of the Command Set where the Commands are registered</param>
        /// <returns>An array of Command data-transfer-objects</returns>
        Task<CommandDTO[]> GetCommandsAsync(Guid commandSetId);
        /// <summary>
        /// Get a Command
        /// </summary>
        /// <param name="id">The Id of the Command to retrieve</param>
        /// <returns>A Command ViewModel</returns>
        Task<CommandVM> GetCommandAsync(Guid id);
        /// <summary>
        /// Create a Command
        /// </summary>
        /// <param name="request">A Command create request</param>
        /// <returns>A Command ViewModel</returns>
        Task<CommandVM> CreateCommandAsync(CommandCreateRequest request);
        /// <summary>
        /// Update a Command
        /// </summary>
        /// <param name="request">The Commandet update request</param>
        /// <returns>A Command ViewModel</returns>
        Task<CommandVM> UpdateCommandAsync(CommandUpdateRequest request);
        /// <summary>
        /// Delete a Command
        /// </summary>
        /// <param name="id">The Id of the Command to be deleted</param>
        /// <returns>True if the delete was successful</returns>
        Task<bool> DeleteCommandAsync(Guid id);

        /// <summary>
        /// Get the Command Replies registered to a Command
        /// </summary>
        /// <param name="commandId">The Id of the Command where the Command Replies are registered</param>
        /// <returns>An array of Command Reply data-transfer objects</returns>
        Task<CommandReplyDTO[]> GetCommandRepliesAsync(Guid commandId);
        /// <summary>
        /// Get a Command Reply
        /// </summary>
        /// <param name="id">The id of the Command Reply to retrieve</param>
        /// <returns>A Command Reply ViewModel</returns>
        Task<CommandReplyVM> GetCommandReplyAsync(Guid id);
        /// <summary>
        /// Create a Command Reply
        /// </summary>
        /// <param name="request">The Command Reply create request</param>
        /// <returns>A Command Reply ViewModel</returns>
        Task<CommandReplyVM> CreateCommandReplyAsync(CommandReplyCreateRequest request);
        /// <summary>
        /// Update a Command Reply
        /// </summary>
        /// <param name="request">The Command Reply update request</param>
        /// <returns>A Command Reply ViewModel</returns>
        Task<CommandReplyVM> UpdateCommandReplyAsync(CommandReplyUpdateRequest request);
        /// <summary>
        /// Delete a Command Reply
        /// </summary>
        /// <param name="id">The Id of the Command Reply to be deleted</param>
        /// <returns>True if the Command Reply was successfully deleted</returns>
        Task<bool> DeleteCommandReplyAsync(Guid id);

        /// <summary>
        /// Get the registered Currencies
        /// </summary>
        /// <returns>An array of Currency data-transfer objects</returns>
        Task<CurrencyDTO[]> GetCurrenciesAsync();
        /// <summary>
        /// Get a Currency
        /// </summary>
        /// <param name="id">The Id of the requested Currency</param>
        /// <returns>A Currency ViewModel</returns>
        Task<CurrencyVM> GetCurrencyAsync(Guid id);
        /// <summary>
        /// Create a Currency
        /// </summary>
        /// <param name="request">The Currency create request</param>
        /// <returns>A Currency ViewModel</returns>
        Task<CurrencyVM> CreateCurrencyAsync(CurrencyCreateRequest request);
        /// <summary>
        /// Update a Currency
        /// </summary>
        /// <param name="request">The Currency update request</param>
        /// <returns>A Currency ViewModel</returns>
        Task<CurrencyVM> UpdateCurrencyAsync(CurrencyUpdateRequest request);
        /// <summary>
        /// Delete a Currency
        /// </summary>
        /// <param name="id">The Id of the Currency to be deleted</param>
        /// <returns>True if the delete was successful</returns>
        Task<bool> DeleteCurrencyAsync(Guid id);

        /// <summary>
        /// Get the registered Currencies Users for all Users registered to the Application User
        /// </summary>
        /// <returns>An array of Currencies Users registered to the Application User</returns>
        Task<CurrenciesUserResponse[]> GetCurrenciesUsersAsync();
        /// <summary>
        /// Get the Currencies User for a User
        /// </summary>
        /// <param name="userId">The Id of the desired User to retrieve the User Currencies</param>
        /// <returns>A Currencies User response</returns>
        Task<CurrenciesUserResponse> GetCurrenciesUserAsync(Guid userId);
        /// <summary>
        /// Get the Currencies Users for a number of Users
        /// </summary>
        /// <param name="userIds">An array of User Ids to retrieve the User Currencies</param>
        /// <returns>A Currencies Users response</returns>
        Task<CurrenciesUsersResponse> GetCurrenciesUsersAsync(Guid[] userIds);
        /// <summary>
        /// Get the Currency User
        /// </summary>
        /// <param name="id">The Id of the desired Currency User to retrieve</param>
        /// <returns>A Currency User ViewModel</returns>
        Task<CurrencyUserVM> GetCurrencyUserAsync(Guid id);
        /// <summary>
        /// Get the specified Currencies Users by their Id
        /// </summary>
        /// <param name="ids">An array of Currency User Ids to retrieve</param>
        /// <returns>A Currency Users response</returns>
        Task<CurrencyUsersResponse> GetCurrencyUsers(Guid[] ids);
        /// <summary>
        /// Create a Currency User Transaction
        /// </summary>
        /// <param name="request">A Currency User Transaction request</param>
        /// <returns>A Currency User ViewModel</returns>
        Task<CurrencyUserVM> CreateCurrencyUserTransaction(CurrencyUserTransactionRequest request);
        /// <summary>
        /// Create a Currency User Transaction for the specified Currency Users
        /// </summary>
        /// <param name="request">A Currency Users Transaction request</param>
        /// <returns>A Currency Users response</returns>
        Task<CurrencyUsersResponse> CreateCurrencyUsersTransactions(CurrencyUsersTransactionRequest request);

        /// <summary>
        /// Get the Paths associated with a Route
        /// </summary>
        /// <param name="routeId">The Route Id that contains the requested Paths</param>
        /// <returns>An array of Path data-transfer objects</returns>
        Task<PathDTO[]> GetPathsAsync(Guid routeId);
        /// <summary>
        /// Get a Path of type None
        /// </summary>
        /// <param name="id">The Id of the Path type None</param>
        /// <returns>A Path ViewModel</returns>
        Task<PathVM> GetPathAsync(Guid id);
        /// <summary>
        /// Create a Path of type None
        /// </summary>
        /// <param name="request">The Path create request</param>
        /// <returns>A Path ViewModel</returns>
        Task<PathVM> CreatePathAsync(PathCreateRequest request);
        /// <summary>
        /// Get a Path of type Server
        /// </summary>
        /// <param name="id">The Id of the Path type Server</param>
        /// <returns>A Path Server ViewModel</returns>
        Task<PathServerVM> GetPathServerAsync(Guid id);
        /// <summary>
        /// Create a Path of type Server
        /// </summary>
        /// <param name="request">The Path Server create request</param>
        /// <returns>A Path Server ViewModel</returns>
        Task<PathServerVM> CreatePathServerAsync(PathServerCreateRequest request);
        /// <summary>
        /// Get a Path of type Channel
        /// </summary>
        /// <param name="id">The Id of the Path type Channel</param>
        /// <returns>A Path Channel ViewModel</returns>
        Task<PathChannelVM> GetPathChannelAsync(Guid id);
        /// <summary>
        /// Create a Path of type Channel
        /// </summary>
        /// <param name="request">The Path Channel create request</param>
        /// <returns>A Path Channel ViewModel</returns>
        Task<PathChannelVM> CreatePathChannelAsync(PathChannelCreateRequest request);
        /// <summary>
        /// Delete a Path
        /// </summary>
        /// <param name="id">The Id of the Path to be deleted</param>
        /// <returns>True if the delete was successful</returns>
        Task<bool> DeletePathAsync(Guid id);

        /// <summary>
        /// Get the Providers
        /// </summary>
        /// <returns>An array of Provider data-transfer objects</returns>
        Task<ProviderDTO[]> GetProvidersAsync();
        /// <summary>
        /// Get a Provider
        /// </summary>
        /// <param name="id">The Id of the requested Provider</param>
        /// <returns>A Provider ViewModel</returns>
        Task<ProviderVM> GetProviderAsync(Guid id);

        /// <summary>
        /// Get the Routes associated with a Path
        /// </summary>
        /// <param name="streamId">The Stream Id that contains the requested Routes</param>
        /// <returns>An array of Route data-transfer objects</returns>
        Task<RouteDTO[]> GetRoutesAsync(Guid id);
        /// <summary>
        /// Get a Route
        /// </summary>
        /// <param name="id">The Id of the Route</param>
        /// <returns>A Route ViewModel</returns>
        Task<RouteVM> GetRouteAsync(Guid id);
        /// <summary>
        /// Create a Route
        /// </summary>
        /// <param name="request">The Route create request</param>
        /// <returns>A Route ViewModel</returns>
        Task<RouteVM> CreateRouteAsync(RouteCreateRequest request);
        /// <summary>
        /// Delete a Route
        /// </summary>
        /// <param name="id">The Id of the Route to be deleted</param>
        /// <returns>True if the delete was successful</returns>
        Task<bool> DeleteRouteAsync(Guid id);

        /// <summary>
        /// Get a Twitch Server by its Id, Twitch Id, or Twitch Server name
        /// </summary>
        /// <param name="id">The Id, Twitch Id, or Twitch Channel name of the requested Twitch Server</param>
        /// <returns>A Twitch Server ViewModel</returns>
        Task<ServerTwitchVM> GetServerTwitchAsync(string id);
        /// <summary>
        /// Get Twitch Servers by their Ids, Twitch Ids, or Twitch Channel names
        /// </summary>
        /// <param name="ids">Array of values of the Ids, Twitch Ids, or Twitch Channel names of the requested Twitch Servers</param>
        /// <returns>A Twitch Servers Response</returns>
        Task<ServersTwitchResponse> GetServersTwitchAsync(string[] ids);
        /// <summary>
        /// Get a Discord Server by its Id or Discord Guild Id
        /// </summary>
        /// <param name="id">The Id or Discord Guild Id of the requested Discord Server</param>
        /// <returns>A Discord Server ViewModel</returns>
        Task<ServerDiscordVM> GetServerDiscordAsync(string id);
        /// <summary>
        /// Get Discord Servers by their Ids, Discord Ids, or Discord Channel names
        /// </summary>
        /// <param name="ids">Array of values of the Ids, Discord Ids, or Discord Channel names of the requested Discord Servers</param>
        /// <returns>A Discord Servers Response</returns>
        Task<ServersDiscordResponse> GetServerDiscordAsync(string[] ids);

        /// <summary>
        /// Get the Users Online in a Twitch Server
        /// </summary>
        /// <param name="serverTwitchId">The Id of the Twitch Server to have its online Users retrieved</param>
        /// <returns>A Twitch Server Users Response</returns>
        Task<ServerUsersTwitchVM> GetServerTwitchUsersAsync(Guid serverTwitchId);
        /// <summary>
        /// Get the Users Online in a Discord Server
        /// </summary>
        /// <param name="serverDiscordId">The Id of the Discord Server to have its online Users retrieved</param>
        /// <returns>A Discord Server Users Response</returns>
        Task<ServerUsersDiscordVM> GetServerDiscordUsersAsync(Guid serverDiscordId);
        /// <summary>
        /// Get the Users and their User Currencies in a Server
        /// </summary>
        /// <param name="serverId">The Id of the Server to retrieve the Users and their User Currencies</param>
        /// <returns>An array of Currencies User ViewModels</returns>
        Task<CurrenciesUserVM[]> GetServerUsersCurrencies(Guid serverId);
        /// <summary>
        /// Get the Discord Channels in a Discord Server
        /// </summary>
        /// <param name="discordServerId">The Discord Server Id to retrieve the registered Channels</param>
        /// <returns>An array of Discord Server Channel data-transfer objects</returns>
        Task<ServerChannelDiscordDTO[]> GetChannelsDiscordAsync(Guid discordServerId);
        /// <summary>
        /// Get a Discord Channel in a Discord 
        /// </summary>
        /// <param name="id">The Discord Channel Id to retrieve</param>
        /// <returns>A Discord Server Channel ViewModel</returns>
        Task<ServerChannelDiscordVM> GetChannelDiscordAsync(Guid id);

        /// <summary>
        /// Get the registered Streams
        /// </summary>
        /// <returns>An array of Stream data-transfer objects</returns>
        Task<StreamDTO[]> GetStreamsAsync();
        /// <summary>
        /// Get a Stream
        /// </summary>
        /// <param name="id">The Id of the requested Stream</param>
        /// <returns>A Stream ViewModel</returns>
        Task<StreamVM> GetStreamAsync(Guid id);
        /// <summary>
        /// Create a Stream
        /// </summary>
        /// <param name="request">The Stream create request</param>
        /// <returns>A Stream ViewModel</returns>
        Task<StreamVM> CreateStreamAsync(StreamCreateRequest request);
        /// <summary>
        /// Update a Stream
        /// </summary>
        /// <param name="request">The Stream update request</param>
        /// <returns>A Stream ViewModel</returns>
        Task<StreamVM> UpdateStreamAsync(StreamUpdateRequest request);
        /// <summary>
        /// Delete a Stream
        /// </summary>
        /// <param name="id">The Id of the Stream to be deleted</param>
        /// <returns>True if the delete was successful</returns>
        Task<bool> DeleteStreamAsync(Guid id);

        /// <summary>
        /// Get the Command Sets registered to a Stream
        /// </summary>
        /// <param name="streamId">The Id of the Stream where the requested Command Sets are registered</param>
        /// <returns>An array of Stream Command Set data-transfer objects</returns>
        Task<StreamCommandSetDTO[]> GetStreamCommandSetsAsync(Guid streamId);
        /// <summary>
        /// Get a Stream Command Set
        /// </summary>
        /// <param name="id">The Id of the Stream Command Set</param>
        /// <returns>A Stream Command Set ViewModel</returns>
        Task<StreamCommandSetVM> GetStreamCommandSetAsync(Guid id);
        /// <summary>
        /// Create a Stream Command Set
        /// </summary>
        /// <param name="request">A Stream Command Set create request</param>
        /// <returns>A Stream Command Set ViewModel</returns>
        Task<StreamCommandSetVM> CreateStreamCommandSetAsync(StreamCommandSetCreateRequest request);
        /// <summary>
        /// Delete a Stream Command Set
        /// </summary>
        /// <param name="id">The Id of the Stream Command Set to delete</param>
        /// <returns>True if the delete was successful</returns>
        Task<bool> DeleteStreamCommandSetAsync(Guid id);

        /// <summary>
        /// Get the Stream Currencies registered to a Stream
        /// </summary>
        /// <param name="streamId">The Stream Id that the Stream Currencies are registered</param>
        /// <returns>An array of Stream Currency data-transfer objects</returns>
        Task<StreamCurrencyDTO[]> GetStreamCurrenciesAsync(Guid streamId);
        /// <summary>
        /// Get a Stream Currency
        /// </summary>
        /// <param name="id">The Id of the requested Stream Currency</param>
        /// <returns>A Stream Currency ViewModel</returns>
        Task<StreamCurrencyVM> GetStreamCurrencyAsync(Guid id);
        /// <summary>
        /// Create a Stream Currency
        /// </summary>
        /// <param name="request">The Stream Currency create request</param>
        /// <returns>A Stream Currency ViewModel</returns>
        Task<StreamCurrencyVM> CreateStreamCurrencyAsync(StreamCurrencyCreateRequest request);
        /// <summary>
        /// Update a Stream Currency
        /// </summary>
        /// <param name="request">The Stream Currency update request</param>
        /// <returns>A Stream Currency ViewModel</returns>
        Task<StreamCurrencyVM> UpdateStreamCurrencyAsync(StreamCurrencyUpdateRequest request);
        /// <summary>
        /// Delete a Stream Currency
        /// </summary>
        /// <param name="id">The Id of the Stream Currency to be deleted</param>
        /// <returns>True if the delete was successful</returns>
        Task<bool> DeleteStreamCurrencyAsync(Guid id);

        /// <summary>
        /// Get the Statuses registered to a Stream Currency
        /// </summary>
        /// <param name="streamCurrencyId">The Stream Currency Id that the Status are registered</param>
        /// <returns>An array of Status data-transfer objects</returns>
        Task<StatusDTO[]> GetStreamCurrencyStatusesAsync(Guid streamCurrencyId);
        /// <summary>
        /// Get a Status
        /// </summary>
        /// <param name="id">The Id of the requested Status</param>
        /// <returns>A Status ViewModel</returns>
        Task<StatusVM> GetStreamCurrencyStatusAsync(Guid id);
        /// <summary>
        /// Create a Status
        /// </summary>
        /// <param name="request">The Status create request</param>
        /// <returns>A Status ViewModel</returns>
        Task<StatusVM> CreateStreamCurrencyStatusAsync(StatusCreateRequest request);
        /// <summary>
        /// Update a Status
        /// </summary>
        /// <param name="request">The Status update request</param>
        /// <returns>A Status ViewModel</returns>
        Task<StatusVM> UpdateStreamCurrencyStatusAsync(StatusUpdateRequest request);
        /// <summary>
        /// Delete a Status
        /// </summary>
        /// <param name="id">The Id of the Status to deleted</param>
        /// <returns>True if the delete was successful</returns>
        Task<bool> DeleteStreamCurrencyStatusAsync(Guid id);

        /// <summary>
        /// Get the Users online in a Stream
        /// </summary>
        /// <param name="streamId">The Stream Id where the Users are registered</param>
        /// <returns>A Stream Users ViewModel</returns>
        Task<StreamUsersVM> GetStreamUsersAsync(Guid streamId);

        /// <summary>
        /// Get the Users and their User Currencies in a Stream
        /// </summary>
        /// <param name="streamId">The Id of the Stream to retrieve the Users and their User Currencies</param>
        /// <returns>An array of Currencies User ViewModels</returns>
        Task<CurrenciesUserVM[]> GetStreamUsersCurrencies(Guid streamId);

        /// <summary>
        /// Get the Users registered to the Application User
        /// </summary>
        /// <returns>An array of User data-transfer objects</returns>
        Task<UserDTO[]> GetUsersAsync();
        /// <summary>
        /// Get multiple Users by their Ids
        /// </summary>
        /// <param name="ids">Ids of the requested Users</param>
        /// <returns>A Users Response</returns>
        Task<UsersResponse> GetUsersAsync(Guid[] ids);
        /// <summary>
        /// Get a Twitch User by their Id, Twitch Id, or Twitch Username
        /// </summary>
        /// <param name="id">The Id, Twitch Id, or Twitch Username of the requested User</param>
        /// <returns>A Twitch User ViewModel</returns>
        Task<UserTwitchVM> GetUserTwitchAsync(string id);
        /// <summary>
        /// Get multiple Twitch Users by their Ids or Twitch Ids
        /// </summary>
        /// <param name="ids">Ids or Twitch Ids of the requested Twitch Users</param>
        /// <returns>A Users Twitch Response</returns>
        Task<UsersTwitchResponse> GetUsersTwitchAsync(string[] ids);
        /// <summary>
        /// Get a Discord User by their Id or Discord Id
        /// </summary>
        /// <param name="id">The Id or Discord Id of the requested User</param>
        /// <returns>A Discord User ViewModel</returns>
        Task<UserDiscordVM> GetUserDiscordAsync(string id);
        /// <summary>
        /// Get multiple Discord Users by their Ids or Discord Ids
        /// </summary>
        /// <param name="ids">The Ids or Discord Ids of the requested Users</param>
        /// <returns>A Users Discord Response</returns>
        Task<UsersDiscordResponse> GetUsersDiscordAsync(string[] ids);
    }
}