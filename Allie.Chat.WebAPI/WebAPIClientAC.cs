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
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Allie.Chat.Lib.DTOs.Servers.Channels;
using Allie.Chat.Lib.ViewModels.Servers.Channels;
using Allie.Chat.Lib.DTOs.Users;
using Allie.Chat.Lib.Responses.Currencies;
using IdentityModel.OidcClient;
using IdentityModel.Client;
using IdentityModel.OidcClient.Results;
using System.Threading;

namespace Allie.Chat.WebAPI
{
    public class WebAPIClientAC : IWebAPIClientAC
    {
        private readonly string _webAPIBaseUrl;

        protected OidcClient _oidcClient;
        protected string _accessToken;
        protected readonly string _identityServerAuthorityUrl;
        protected readonly IHttpClientFactory _httpClientFactory;

        public WebAPIClientAC(string accessToken = "", string webAPIBaseUri = "https://api.allie.chat", string identityServerAuthorityUrl = "https://identity.allie.chat", IHttpClientFactory httpClientFactory = null)
        {
            _httpClientFactory = httpClientFactory;
            _accessToken = accessToken;
            _webAPIBaseUrl = webAPIBaseUri;
            _identityServerAuthorityUrl = identityServerAuthorityUrl;
        }

        public async Task<TokenResponse> GetAccessTokenResourceOwnerPasswordAsync(string clientId, string clientSecret,
            string scopes, string username, string password, CancellationToken cancellationToken = default)
        {
            using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
            {
                var disco = await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
                {
                    Address = _identityServerAuthorityUrl,
                }, cancellationToken);

                var response = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
                {
                    Address = disco.TokenEndpoint,
                    ClientId = clientId,
                    ClientSecret = clientSecret,
                    UserName = username,
                    Password = password,
                    Scope = scopes,
                }, cancellationToken);

                if (response != null)
                {
                    _accessToken = response.AccessToken;
                }

                return response;
            }
        }
        public async Task<LoginResult> GetAccessTokenAuthCodeAsync(string clientId, string clientSecret, string scopes, CancellationToken cancellationToken = default)
        {
            // create a redirect URI using an available port on the loopback address.
            // requires the OP to allow random ports on 127.0.0.1 - otherwise set a static port
            var browser = new SystemBrowser(45656);
            string redirectUri = string.Format($"http://127.0.0.1:{browser.Port}");

            var options = new OidcClientOptions
            {
                Authority = _identityServerAuthorityUrl,
                ClientId = clientId,
                RedirectUri = redirectUri,
                Scope = scopes,
                FilterClaims = false,
                Browser = browser,
                RefreshTokenInnerHttpHandler = new HttpClientHandler(),
                Flow = OidcClientOptions.AuthenticationFlow.AuthorizationCode,
                ClientSecret = clientSecret
            };

            if (!cancellationToken.IsCancellationRequested)
            {
                _oidcClient = new OidcClient(options);
                var result = await _oidcClient.LoginAsync(new LoginRequest());

                if (result != null)
                {
                    _accessToken = result.AccessToken;
                }

                return result;
            }

            return null;
        }
        public async Task<LoginResult> GetAccessTokenNativePKCEAsync(string clientId, string scopes, CancellationToken cancellationToken = default)
        {
            // create a redirect URI using an available port on the loopback address.
            // requires the OP to allow random ports on 127.0.0.1 - otherwise set a static port
            var browser = new SystemBrowser(45656);
            string redirectUri = string.Format($"http://127.0.0.1:{browser.Port}");

            var options = new OidcClientOptions
            {
                Authority = _identityServerAuthorityUrl,
                ClientId = clientId,
                RedirectUri = redirectUri,
                Scope = scopes,
                FilterClaims = false,
                Browser = browser,
                RefreshTokenInnerHttpHandler = new HttpClientHandler(),
                Flow = OidcClientOptions.AuthenticationFlow.AuthorizationCode,
            };

            _oidcClient = new OidcClient(options);
            var result = await _oidcClient.LoginAsync(new LoginRequest());

            if (!cancellationToken.IsCancellationRequested)
            {
                if (result != null)
                {
                    _accessToken = result.AccessToken;
                }

                return result;
            }

            return null;
        }

        public async Task<RefreshTokenResult> RefreshAccessTokenAuthCodeOrNativeAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            if (_oidcClient != null && !cancellationToken.IsCancellationRequested)
            {
                var result = await _oidcClient.RefreshTokenAsync(refreshToken);

                if (result != null)
                {
                    _accessToken = result.AccessToken;
                }

                return result;
            }

            return null;
        }
        public async Task<TokenResponse> RefreshAccessTokenResourceOwnerPasswordAsync(string clientId,
            string clientSecret, string refreshToken, CancellationToken cancellationToken = default)
        {
            using var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _identityServerAuthorityUrl,
            }, cancellationToken);

            var result = await client.RequestRefreshTokenAsync(new RefreshTokenRequest
            {
                Address = disco.TokenEndpoint,
                RefreshToken = refreshToken,
                ClientId = clientId,
                ClientSecret = clientSecret
            }, cancellationToken);

            if (result != null)
            {
                _accessToken = result.AccessToken;
            }

            return result;
        }

        public async Task<UserInfoResult> GetUserInfoAuthCodeOrNativeAsync(CancellationToken cancellationToken = default)
        {
            if (!cancellationToken.IsCancellationRequested && _oidcClient != null)
            {
                return await _oidcClient.GetUserInfoAsync(_accessToken);
            }

            return null;
        }
        public async Task<UserInfoResponse> GetUserInfoResourceOwnerPasswordAsync(CancellationToken cancellationToken = default)
        {
            using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
            {
                var disco = await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
                {
                    Address = _identityServerAuthorityUrl,
                }, cancellationToken);

                return await client.GetUserInfoAsync(new UserInfoRequest
                {
                    Address = disco.UserInfoEndpoint,
                    Token = _accessToken
                }, cancellationToken);
            }
        }

        public async Task<IntrospectionResponse> IntrospectAccessTokenAsync(string clientId, string clientSecret, string apiName, string apiSecret, CancellationToken cancellationToken = default)
        {
            using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
            {
                var disco = await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
                {
                    Address = _identityServerAuthorityUrl,
                }, cancellationToken);

                client.SetBasicAuthentication(apiName, apiSecret);

                return await client.IntrospectTokenAsync(new TokenIntrospectionRequest
                {
                    Address = disco.IntrospectionEndpoint,
                    ClientId = clientId,
                    ClientSecret = clientSecret,
                    Token = _accessToken,
                }, cancellationToken);
            }
        }

        public virtual void SetAccessToken(string accessToken)
        {
            _accessToken = accessToken;
        }

        /// <summary>
        /// Get the registered Api Resources
        /// </summary>
        /// <returns>An array of Api Resource data-transfer objects</returns>
        public async virtual Task<ApiResourceDTO[]> GetApiResourcesAsync(CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/ApiResources", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<ApiResourceDTO[]>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Get the registered Api Resource
        /// </summary>
        /// <param name="id">The Id of the Api Resource to retrieve</param>
        /// <returns>An Api Resource ViewModel</returns>
        public async virtual Task<ApiResourceVM> GetApiResourceAsync(int id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/ApiResources/{id}", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<ApiResourceVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Create an Api Resource
        /// </summary>
        /// <param name="request">The Api Resource create request</param>
        /// <returns>The created Api Resource ViewModel</returns>
        public async virtual Task<ApiResourceVM> CreateApiResourceAsync(ApiResourceCreateRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.PostAsync($"{_webAPIBaseUrl}/ApiResources", new JsonContent(request), cancellationToken);

                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        return JsonConvert.DeserializeObject<ApiResourceVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Update an Api Resource
        /// </summary>
        /// <param name="request"The Api Resource update request></param>
        /// <returns>The updated Api Resource ViewModel</returns>
        public async virtual Task<ApiResourceVM> UpdateApiResourceAsync(ApiResourceUpdateRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.PutAsync($"{_webAPIBaseUrl}/ApiResources/{request.Id}", new JsonContent(request), cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<ApiResourceVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Delete an Api Resource
        /// </summary>
        /// <param name="id">Id of the requested Api Resource to delete</param>
        /// <returns>True if the delete was successful</returns>
        public async virtual Task<bool> DeleteApiResourceAsync(int id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.DeleteAsync($"{_webAPIBaseUrl}/ApiResources/{id.ToString()}", cancellationToken);

                    return response.StatusCode == HttpStatusCode.NoContent;
                }
            }
            catch
            { }

            return false;
        }
        /// <summary>
        /// Reset an Api Resource's Secrets
        /// </summary>
        /// <param name="id">The Id of the Api Resource to reset its secrets</param>
        /// <returns>True if the Api Resource Secrets were successfully reset</returns>
        public async virtual Task<bool> ResetApiResourceSecretsAsync(int id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var result = await client.PostAsync($"{_webAPIBaseUrl}/ApiResources/ResetSecrets/{id}", new JsonContent(null), cancellationToken);

                    return result.IsSuccessStatusCode;
                }
            }
            catch
            { }

            return false;
        }

        /// <summary>
        /// Retrieve the Authorized Application User
        /// </summary>
        /// <returns>An Application Udser ViewModel</returns>
        public async Task<ApplicationUserVM> GetApplicationUserAsync(CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var user = await client.GetStringAsync(_webAPIBaseUrl + "/ApplicationUser", cancellationToken);

                    if (!string.IsNullOrWhiteSpace(user))
                    {
                        return JsonConvert.DeserializeObject<ApplicationUserVM>(user);
                    }
                }
            }
            catch
            { }

            return null;
        }

        /// <summary>
        /// Get the registered Bots
        /// </summary>
        /// <returns>An array of Bot data-transfer objects registered to the Application User</returns>
        public async virtual Task<BotDTO[]> GetBotsAsync(CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/Bots", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<BotDTO[]>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Get the registered Bot by token
        /// </summary>
        /// <param name="token">The OAuth Token of the requested Bot</param>
        /// <returns>A Bot ViewModel</returns>
        public async virtual Task<BotVM> GetBotAsync(string token, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/Bots/{token}", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<BotWSVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Get a registered Twitch Bot
        /// </summary>
        /// <param name="id">Id of the requested Twitch Bot</param>
        /// <returns>The Twitch Bot ViewModel</returns>
        public async virtual Task<BotTwitchVM> GetBotTwitchAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/Bots/Twitch/{id.ToString()}", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<BotTwitchVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Get a registered Discord Bot
        /// </summary>
        /// <param name="id">The Id of the Discord Bot</param>
        /// <returns>A Discord Bot ViewModel</returns>
        public async virtual Task<BotDiscordVM> GetBotDiscordAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/Bots/Discord/{id.ToString()}", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<BotDiscordVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Get a registered Tcp Bot
        /// </summary>
        /// <param name="id">The Id of the requested Tcp Bot</param>
        /// <returns>The Tcp Bot ViewModel</returns>
        public async virtual Task<BotTcpVM> GetBotTcpAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/Bots/Tcp/{id.ToString()}", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<BotTcpVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Get a registered Tcp Bot
        /// </summary>
        /// <param name="token">The OAuth token of the requested Tcp Bot</param>
        /// <returns>The Tcp Bot ViewModel</returns>
        public async virtual Task<BotTcpVM> GetBotTcpAsync(string token, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/Bots/Tcp/{token}", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<BotTcpVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Create a Tcp Bot
        /// </summary>
        /// <param name="request">The Tcp Bot create request</param>
        /// <returns>A Tcp Bot ViewModel</returns>
        public async virtual Task<BotTcpVM> PostBotTcpAsync(BotTcpCreateRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.PostAsync($"{_webAPIBaseUrl}/Bots/Tcp", new JsonContent(request), cancellationToken);

                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        return JsonConvert.DeserializeObject<BotTcpVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Update a Tcp Bot
        /// </summary>
        /// <param name="request">The Tcp Bot update request</param>
        /// <returns>A Tcp Bot ViewModel</returns>
        public async virtual Task<BotTcpVM> PutBotTcpAsync(BotTcpUpdateRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.PutAsync($"{_webAPIBaseUrl}/Bots/Tcp/{request.Id}", new JsonContent(request), cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<BotTcpVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Get a Websocket Bot
        /// </summary>
        /// <param name="id">The Id of the requested Websocket Bot</param>
        /// <returns>A Websocket Bot ViewModel</returns>
        public async virtual Task<BotWSVM> GetBotWebsocketAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/Bots/Websocket/{id.ToString()}", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<BotWSVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Get a Websocket Bot
        /// </summary>
        /// <param name="token">The OAuth Token of the requested Websocket Bot</param>
        /// <returns>A Websocket Bot ViewModel</returns>
        public async virtual Task<BotWSVM> GetBotWebsocketAsync(string token, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/Bots/Websocket/{token}", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<BotWSVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Create a Websocket Bot
        /// </summary>
        /// <param name="request">A Websocket Bot create request</param>
        /// <returns>A Websocket Bot ViewModel</returns>
        public virtual async Task<BotWSVM> CreateBotWebsocketAsync(BotWSCreateRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.PostAsync($"{_webAPIBaseUrl}/Bots/Websocket", new JsonContent(request), cancellationToken);

                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        return JsonConvert.DeserializeObject<BotWSVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Update a Websocket Bot
        /// </summary>
        /// <param name="request">The Websocket Bot update request</param>
        /// <returns>A Websocket Bot ViewModel</returns>
        public virtual async Task<BotWSVM> UpdateBotWebsocketAsync(BotWSUpdateRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.PutAsync($"{_webAPIBaseUrl}/Bots/Websocket/{request.Id}", new JsonContent(request), cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<BotWSVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Delete a Websocket Bot
        /// </summary>
        /// <param name="request">The Websocket Bot update request</param>
        /// <returns>True if the delete was successful</returns>
        public async virtual Task<bool> DeleteBotAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.DeleteAsync($"{_webAPIBaseUrl}/Bots/{id.ToString()}".ToLower(), cancellationToken);

                    return response.StatusCode == HttpStatusCode.NoContent;
                }
            }
            catch
            { }

            return false;
        }

        /// <summary>
        /// Get the registered Client Applications
        /// </summary>
        /// <returns>An array of Client Application data-transfer objects</returns>
        public async virtual Task<ClientApplicationDTO[]> GetClientApplicationsAsync(CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/ClientApplications", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<ClientApplicationDTO[]>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Get an Authorization Code Client Application
        /// </summary>
        /// <param name="id">The Id of the requested Authorization Code Client Application</param>
        /// <returns>An Authorization Code Client Application ViewModel</returns>
        public async virtual Task<ClientApplicationAuthCodeVM> GetClientApplicationAuthCodeAsync(int id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/ClientApplications/AuthCode/{id.ToString()}", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<ClientApplicationAuthCodeVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Create an Authorization Code Client Application
        /// </summary>
        /// <param name="request">The Authorization Code Client Application create request</param>
        /// <returns>An Authorization Code Client Application ViewModel</returns>
        public async virtual Task<ClientApplicationAuthCodeVM> CreateClientApplicationAuthCodeAsync(ClientApplicationAuthCodeCreateRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.PostAsync($"{_webAPIBaseUrl}/ClientApplications/AuthCode", new JsonContent(request), cancellationToken);

                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        return JsonConvert.DeserializeObject<ClientApplicationAuthCodeVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Update an Authorization Code Client Application
        /// </summary>
        /// <param name="request">The Authorization Code Client Application update request</param>
        /// <returns>An Authorization Code Client Application ViewModel</returns>
        public async virtual Task<ClientApplicationAuthCodeVM> UpdateClientApplicationAuthCodeAsync(ClientApplicationAuthCodeUpdateRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.PutAsync($"{_webAPIBaseUrl}/ClientApplications/AuthCode/{request.Id}", new JsonContent(request), cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<ClientApplicationAuthCodeVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Get an Implicit Client Application
        /// </summary>
        /// <param name="id">The Id of the requested Implicit Client Application</param>
        /// <returns>An Implicit Client Application ViewModel</returns>
        public async virtual Task<ClientApplicationImplicitVM> GetClientApplicationImplicitAsync(int id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/ClientApplications/Implicit/{id.ToString()}", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<ClientApplicationImplicitVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Create an Implicit Client Application
        /// </summary>
        /// <param name="request">The Implicit Client create request</param>
        /// <returns>An Implicit Client Application ViewModel</returns>
        public async virtual Task<ClientApplicationImplicitVM> CreateClientApplicationImplicitAsync(ClientApplicationImplicitCreateRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.PostAsync($"{_webAPIBaseUrl}/ClientApplications/Implicit", new JsonContent(request), cancellationToken);

                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        return JsonConvert.DeserializeObject<ClientApplicationImplicitVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Update an Implicit Client Application
        /// </summary>
        /// <param name="request">The Implicit Client update request</param>
        /// <returns>An Implicit Client Application ViewModel</returns>
        public async virtual Task<ClientApplicationImplicitVM> UpdateClientApplicationImplicitAsync(ClientApplicationImplicitUpdateRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.PutAsync($"{_webAPIBaseUrl}/ClientApplications/Implicit/{request.Id}", new JsonContent(request), cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<ClientApplicationImplicitVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Get a Resource Owner Password Credentials Client Application
        /// </summary>
        /// <param name="id">The Id of the requested Resource Owner Password Credentials Client Application</param>
        /// <returns>A Resource Owner Password Credentials Client Application ViewModel</returns>
        public async virtual Task<ClientApplicationROPasswordVM> GetClientApplicationPasswordAsync(int id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var clientApplicationPassword = await client.GetStringAsync($"{_webAPIBaseUrl}/ClientApplications/ROPassword/{id.ToString()}", cancellationToken);

                    if (!string.IsNullOrWhiteSpace(clientApplicationPassword))
                    {
                        return JsonConvert.DeserializeObject<ClientApplicationROPasswordVM>(clientApplicationPassword);
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Create a Resource Owner Password Credentials Client Application
        /// </summary>
        /// <param name="request">A Resource Owner Password Credentials Client Application create request</param>
        /// <returns>A Resource Owner Password Credentials Client Application ViewModel</returns>
        public async virtual Task<ClientApplicationROPasswordVM> CreateClientApplicationPasswordAsync(ClientApplicationROPasswordCreateRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.PostAsync($"{_webAPIBaseUrl}/ClientApplications/ROPassword", new JsonContent(request), cancellationToken);

                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        return JsonConvert.DeserializeObject<ClientApplicationROPasswordVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Update a Resource Owner Password Credentials Client Application
        /// </summary>
        /// <param name="request">A Resource Owner Password Credentials Client Application update request</param>
        /// <returns>A Resource Owner Password Credentials Client Application ViewModel</returns>
        public async virtual Task<ClientApplicationROPasswordVM> UpdateClientApplicationPasswordAsync(ClientApplicationROPasswordUpdateRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.PutAsync($"{_webAPIBaseUrl}/ClientApplications/ROPassword/{request.Id}", new JsonContent(request), cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<ClientApplicationROPasswordVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Get a Native PKCE / Authorization Code Client Application
        /// </summary>
        /// <param name="id">The Id of the requested Native PKCE / Authorization Code Client Application</param>
        /// <returns>A Native PKCE / Authorization Code Client APplication ViewModel</returns>
        public async virtual Task<ClientApplicationPKCEVM> GetClientApplicationNativePKCEAsync(int id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/ClientApplications/PKCE/{id.ToString()}", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<ClientApplicationPKCEVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Create a Native PKCE / Authorization Code Client Application
        /// </summary>
        /// <param name="request">The Native PKCE / Authorization Code Client Application create request</param>
        /// <returns>The Native PKCE / Authorization Code Client Application ViewModel</returns>
        public async virtual Task<ClientApplicationPKCEVM> CreateClientApplicationNativePKCEAsync(ClientApplicationPKCECreateRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.PostAsync($"{_webAPIBaseUrl}/ClientApplications/PKCE", new JsonContent(request), cancellationToken);

                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        return JsonConvert.DeserializeObject<ClientApplicationPKCEVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Update a Native PKCE / Authorization Code Client Application
        /// </summary>
        /// <param name="request">The Native PKCE / Authorization Code Client Application update request</param>
        /// <returns>The Native PKCE / Authorization Code Client Application ViewModel</returns>
        public async virtual Task<ClientApplicationPKCEVM> UpdateClientApplicationNativePKCEAsync(ClientApplicationPKCEUpdateRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.PutAsync($"{_webAPIBaseUrl}/ClientApplications/PKCE/{request.Id}", new JsonContent(request), cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<ClientApplicationPKCEVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Delete a Client Application
        /// </summary>
        /// <param name="id">The Id of the Client Application to delete</param>
        /// <returns>A Native PKCE / Authorization Code Client Application ViewModel</returns>
        public async virtual Task<bool> DeleteClientApplicationAsync(int id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.DeleteAsync($"{_webAPIBaseUrl}/ClientApplications/{id.ToString()}", cancellationToken);

                    return response.StatusCode == HttpStatusCode.NoContent;
                }
            }
            catch
            { }

            return false;
        }
        /// Reset a Client Application's secrets
        /// </summary>
        /// <param name="id">The Id of the Client Application to reset its secrets</param>
        /// <returns>True if the Client Application's secrets were reset</returns>
        public async Task<bool> ResetClientApplicationSecretsAsync(int id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.PostAsync($"{_webAPIBaseUrl}/ClientApplications/ResetSecrets/{id}", new JsonContent(null), cancellationToken);

                    return response.StatusCode == HttpStatusCode.NoContent;
                }
            }
            catch
            { }

            return false;
        }

        /// <summary>
        /// Get the registered Command Sets
        /// </summary>
        /// <returns>An array of Command Set data-transfer objects</returns>
        public async virtual Task<CommandSetDTO[]> GetCommandSetsAsync(CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/CommandSets", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<CommandSetDTO[]>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Get a Command Set
        /// </summary>
        /// <param name="id">The Id of the requested Command Set</param>
        /// <returns>A Command Set ViewModel</returns>
        public async virtual Task<CommandSetVM> GetCommandSetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/CommandSets/{id}", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<CommandSetVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Create a Command Set
        /// </summary>
        /// <param name="request">The Command Set create request</param>
        /// <returns>A Command Set ViewModel</returns>
        public async virtual Task<CommandSetVM> CreateCommandSetAsync(CommandSetCreateRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.PostAsync($"{_webAPIBaseUrl}/CommandSets", new JsonContent(request), cancellationToken);

                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        return JsonConvert.DeserializeObject<CommandSetVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Update a Command Set
        /// </summary>
        /// <param name="request">The Command Set update request</param>
        /// <returns>A Command Set ViewModel</returns>
        public async virtual Task<CommandSetVM> UpdateCommandSetAsync(CommandSetUpdateRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.PutAsync($"{_webAPIBaseUrl}/CommandSets/{request.Id}", new JsonContent(request), cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<CommandSetVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Delete a Command Set
        /// </summary>
        /// <param name="id">The Id of the Command Set to be deleted</param>
        /// <returns>True if the delete was successful</returns>
        public async virtual Task<bool> DeleteCommandSetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.DeleteAsync($"{_webAPIBaseUrl}/CommandSets/{id.ToString()}", cancellationToken);

                    return response.StatusCode == HttpStatusCode.NoContent;
                }
            }
            catch
            { }

            return false;
        }

        /// <summary>
        /// Get the Commands registered to a Command Set
        /// </summary>
        /// <param name="commandSetId">The Id of the Command Set where the Commands are registered</param>
        /// <returns>An array of Command data-transfer objects</returns>
        public async virtual Task<CommandDTO[]> GetCommandsAsync(Guid commandSetId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/CommandSets/Commands/{commandSetId}", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<CommandDTO[]>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Get a Command
        /// </summary>
        /// <param name="id">The Id of the Command to retrieve</param>
        /// <returns>A Command ViewModel</returns>
        public async virtual Task<CommandVM> GetCommandAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/CommandSets/Command/{id}", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<CommandVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Create a Command
        /// </summary>
        /// <param name="request">A Command create request</param>
        /// <returns>A Command ViewModel</returns>
        public async virtual Task<CommandVM> CreateCommandAsync(CommandCreateRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.PostAsync($"{_webAPIBaseUrl}/CommandSets/Commands", new JsonContent(request), cancellationToken);

                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        return JsonConvert.DeserializeObject<CommandVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Update a Command
        /// </summary>
        /// <param name="request">The Commandet update request</param>
        /// <returns>A Command ViewModel</returns>
        public async virtual Task<CommandVM> UpdateCommandAsync(CommandUpdateRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.PutAsync($"{_webAPIBaseUrl}/CommandSets/Commands/{request.Id}", new JsonContent(request), cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<CommandVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Delete a Command
        /// </summary>
        /// <param name="id">The Id of the Command to be deleted</param>
        /// <returns>True if the delete was successful</returns>
        public async virtual Task<bool> DeleteCommandAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.DeleteAsync($"{_webAPIBaseUrl}/CommandSets/Commands/{id.ToString()}", cancellationToken);

                    return response.StatusCode == HttpStatusCode.NoContent;
                }
            }
            catch
            { }

            return false;
        }

        /// <summary>
        /// Get the Command Replies registered to a Command
        /// </summary>
        /// <param name="commandId">The Id of the Command where the Command Replies are registered</param>
        /// <returns>An array of Command Reply data-transfer objects</returns>
        public async virtual Task<CommandReplyDTO[]> GetCommandRepliesAsync(Guid commandId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/CommandSets/Commands/CommandReplies/{commandId}", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<CommandReplyDTO[]>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Get a Command Reply
        /// </summary>
        /// <param name="id">The id of the Command Reply to retrieve</param>
        /// <returns>A Command Reply ViewModel</returns>
        public async virtual Task<CommandReplyVM> GetCommandReplyAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/CommandSets/Command/CommandReply/{id}", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<CommandReplyVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Create a Command Reply
        /// </summary>
        /// <param name="request">The Command Reply create request</param>
        /// <returns>A Command Reply ViewModel</returns>
        public async virtual Task<CommandReplyVM> CreateCommandReplyAsync(CommandReplyCreateRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.PostAsync($"{_webAPIBaseUrl}/CommandSets/Commands/CommandReplies", new JsonContent(request), cancellationToken);

                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        return JsonConvert.DeserializeObject<CommandReplyVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Update a Command Reply
        /// </summary>
        /// <param name="request">The Command Reply update request</param>
        /// <returns>A Command Reply ViewModel</returns>
        public async virtual Task<CommandReplyVM> UpdateCommandReplyAsync(CommandReplyUpdateRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.PutAsync($"{_webAPIBaseUrl}/CommandSets/Commands/CommandReplies/{request.Id}", new JsonContent(request), cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<CommandReplyVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Delete a Command Reply
        /// </summary>
        /// <param name="id">The Id of the Command Reply to be deleted</param>
        /// <returns>True if the Command Reply was successfully deleted</returns>
        public async virtual Task<bool> DeleteCommandReplyAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.DeleteAsync($"{_webAPIBaseUrl}/CommandSets/Commands/CommandReplies{id.ToString()}", cancellationToken);

                    return response.StatusCode == HttpStatusCode.NoContent;
                }
            }
            catch
            { }

            return false;
        }

        /// <summary>
        /// Get the registered Currencies
        /// </summary>
        /// <returns>An array of Currency data-transfer objects</returns>
        public async virtual Task<CurrencyDTO[]> GetCurrenciesAsync(CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/Currencies", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<CurrencyDTO[]>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Get a Currency
        /// </summary>
        /// <param name="id">The Id of the requested Currency</param>
        /// <returns>A Currency ViewModel</returns>
        public async virtual Task<CurrencyVM> GetCurrencyAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/Currencies/{id}", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<CurrencyVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Create a Currency
        /// </summary>
        /// <param name="request">The Currency create request</param>
        /// <returns>A Currency ViewModel</returns>
        public async virtual Task<CurrencyVM> CreateCurrencyAsync(CurrencyCreateRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.PostAsync($"{_webAPIBaseUrl}/Currencies", new JsonContent(request), cancellationToken);

                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        return JsonConvert.DeserializeObject<CurrencyVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Update a Currency
        /// </summary>
        /// <param name="request">The Currency update request</param>
        /// <returns>A Currency ViewModel</returns>
        public async virtual Task<CurrencyVM> UpdateCurrencyAsync(CurrencyUpdateRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.PutAsync($"{_webAPIBaseUrl}/Currencies/{request.Id}", new JsonContent(request), cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<CurrencyVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Delete a Currency
        /// </summary>
        /// <param name="id">The Id of the Currency to be deleted</param>
        /// <returns>True if the delete was successful</returns>
        public async virtual Task<bool> DeleteCurrencyAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.DeleteAsync($"{_webAPIBaseUrl}/Currencies/{id.ToString()}", cancellationToken);

                    return response.StatusCode == HttpStatusCode.NoContent;
                }
            }
            catch
            { }

            return false;
        }

        /// <summary>
        /// Get the registered Currencies Users for all Users registered to the Application User
        /// </summary>
        /// <returns>An array of Currencies Users registered to the Application User</returns>
        public async virtual Task<CurrenciesUserResponse[]> GetCurrenciesUsersAsync(CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/Currencies/Users", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<CurrenciesUserResponse[]>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Get the Currencies User for a User
        /// </summary>
        /// <param name="userId">The Id of the desired User to retrieve the Currencies User</param>
        /// <returns>A Currencies User response</returns>
        public async virtual Task<CurrenciesUserResponse> GetCurrenciesUserAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/Currencies/User/{userId}", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<CurrenciesUserResponse>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Get the Currencies Users for a number of Users
        /// </summary>
        /// <param name="userIds">An array of User Ids to retrieve the User Currencies</param>
        /// <returns>A Currencies Users response</returns>
        public async virtual Task<CurrenciesUsersResponse> GetCurrenciesUsersAsync(Guid[] userIds, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                var sb = new StringBuilder();

                for (int i = 0; i < userIds.Length; i++)
                {
                    sb.Append(userIds[i].ToString());

                    if (i != userIds.Length - 1)
                    {
                        sb.Append(",");
                    }
                }

                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/Currencies/Users?userIds={sb.ToString()}", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<CurrenciesUsersResponse>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Get the Currency User
        /// </summary>
        /// <param name="id">The Id of the desired Currency User to retrieve</param>
        /// <returns>A Currency User ViewModel</returns>
        public async virtual Task<CurrencyUserVM> GetCurrencyUserAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/Currency/User/{id}", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<CurrencyUserVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Get the specified Currencies Users by their Id
        /// </summary>
        /// <param name="ids">An array of Currency User Ids to retrieve</param>
        /// <returns>A Currency Users response</returns>
        public virtual async Task<CurrencyUsersResponse> GetCurrencyUsers(Guid[] ids, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                var sb = new StringBuilder();

                for (int i = 0; i < ids.Length; i++)
                {
                    sb.Append(ids[i].ToString());

                    if (i != ids.Length - 1)
                    {
                        sb.Append(",");
                    }
                }

                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/Currency/Users?ids={sb.ToString()}", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<CurrencyUsersResponse>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Create a Currency User Transaction
        /// </summary>
        /// <param name="request">A Currency User Transaction request</param>
        /// <returns>A Currency User ViewModel</returns>
        public async virtual Task<CurrencyUserVM> CreateCurrencyUserTransaction(CurrencyUserTransactionRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.PostAsync($"{_webAPIBaseUrl}/Currency/User/Transaction", new JsonContent(request), cancellationToken);

                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        return JsonConvert.DeserializeObject<CurrencyUserVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Create a Currency User Transaction for the specified Currency Users
        /// </summary>
        /// <param name="request">A Currency Users Transaction request</param>
        /// <returns>A Currency Users response</returns>
        public async virtual Task<CurrencyUsersResponse> CreateCurrencyUsersTransactions(CurrencyUsersTransactionRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.PostAsync($"{_webAPIBaseUrl}/Currency/Users/Transaction", new JsonContent(request), cancellationToken);

                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        return JsonConvert.DeserializeObject<CurrencyUsersResponse>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }

        /// <summary>
        /// Get the Paths associated with a Route
        /// </summary>
        /// <param name="routeId">The Route Id that contains the requested Paths</param>
        /// <returns>An array of Path data-transfer objects</returns>
        public async virtual Task<PathDTO[]> GetPathsAsync(Guid routeId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/Paths/{routeId.ToString()}", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<PathDTO[]>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Get a Path of type None
        /// </summary>
        /// <param name="id">The Id of the Path type None</param>
        /// <returns>A Path ViewModel</returns>
        public async virtual Task<PathVM> GetPathAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/Paths/Path/{id.ToString()}", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<PathVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Create a Path of type None
        /// </summary>
        /// <param name="request">The Path create request</param>
        /// <returns>A Path ViewModel</returns>
        public async virtual Task<PathVM> CreatePathAsync(PathCreateRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.PostAsync($"{_webAPIBaseUrl}/Paths/Path", new StringContent(JsonConvert.SerializeObject(request)), cancellationToken);

                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        return JsonConvert.DeserializeObject<PathVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Get a Path of type Server
        /// </summary>
        /// <param name="id">The Id of the Path type Server</param>
        /// <returns>A Path Server ViewModel</returns>
        public async virtual Task<PathServerVM> GetPathServerAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/Paths/Server/{id.ToString()}", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<PathServerVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Create a Path of type Server
        /// </summary>
        /// <param name="request">The Path Server create request</param>
        /// <returns>A Path Server ViewModel</returns>
        public async virtual Task<PathServerVM> CreatePathServerAsync(PathServerCreateRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.PostAsync($"{_webAPIBaseUrl}/Paths/Server", new StringContent(JsonConvert.SerializeObject(request)), cancellationToken);

                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        return JsonConvert.DeserializeObject<PathServerVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Get a Path of type Channel
        /// </summary>
        /// <param name="id">The Id of the Path type Channel</param>
        /// <returns>A Path Channel ViewModel</returns>
        public async virtual Task<PathChannelVM> GetPathChannelAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/Paths/Channel/{id.ToString()}", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<PathChannelVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Create a Path of type Channel
        /// </summary>
        /// <param name="request">The Path Channel create request</param>
        /// <returns>A Path Channel ViewModel</returns>
        public async virtual Task<PathChannelVM> CreatePathChannelAsync(PathChannelCreateRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.PostAsync($"{_webAPIBaseUrl}/Paths/Channel", new StringContent(JsonConvert.SerializeObject(request)), cancellationToken);

                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        return JsonConvert.DeserializeObject<PathChannelVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Delete a Path
        /// </summary>
        /// <param name="id">The Id of the Path to be deleted</param>
        /// <returns>True if the delete was successful</returns>
        public async virtual Task<bool> DeletePathAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.DeleteAsync($"{_webAPIBaseUrl}/Paths/{id.ToString()}", cancellationToken);

                    return response.StatusCode == HttpStatusCode.NoContent;
                }
            }
            catch
            { }

            return false;
        }

        /// <summary>
        /// Get the Providers
        /// </summary>
        /// <returns>An array of Provider data-transfer objects</returns>
        public async virtual Task<ProviderDTO[]> GetProvidersAsync(CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/Providers", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<ProviderDTO[]>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Get a Provider
        /// </summary>
        /// <param name="id">The Id of the requested Provider</param>
        /// <returns>A Provider ViewModel</returns>
        public async virtual Task<ProviderVM> GetProviderAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/Providers/{id}", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<ProviderVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }

        /// <summary>
        /// Get the Routes associated with a Path
        /// </summary>
        /// <param name="streamId">The Stream Id that contains the requested Routes</param>
        /// <returns>An array of Route data-transfer objects</returns>
        public async virtual Task<RouteDTO[]> GetRoutesAsync(Guid streamId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/Routes/{streamId.ToString()}", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<RouteDTO[]>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Get a Route
        /// </summary>
        /// <param name="id">The Id of the Route</param>
        /// <returns>A Route ViewModel</returns>
        public async virtual Task<RouteVM> GetRouteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/Route/{id.ToString()}", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<RouteVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Create a Route
        /// </summary>
        /// <param name="request">The Route create request</param>
        /// <returns>A Route ViewModel</returns>
        public async virtual Task<RouteVM> CreateRouteAsync(RouteCreateRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.PostAsync($"{_webAPIBaseUrl}/Routes", new StringContent(JsonConvert.SerializeObject(request)), cancellationToken);

                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        return JsonConvert.DeserializeObject<RouteVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Delete a Route
        /// </summary>
        /// <param name="id">The Id of the Route to be deleted</param>
        /// <returns>True if the delete was successful</returns>
        public async virtual Task<bool> DeleteRouteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.DeleteAsync($"{_webAPIBaseUrl}/Routes/{id.ToString()}", cancellationToken);

                    return response.StatusCode == HttpStatusCode.NoContent;
                }
            }
            catch
            { }

            return false;
        }

        /// <summary>
        /// Get a Twitch Server by its Id, Twitch Id, or Twitch Server name
        /// </summary>
        /// <param name="id">The Id, Twitch Id, or Twitch Channel name of the requested Twitch Server</param>
        /// <returns>A Twitch Server ViewModel</returns>
        public async virtual Task<ServerTwitchVM> GetServerTwitchAsync(string id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/Server/Twitch/{id}", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<ServerTwitchVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Get Twitch Servers by their Ids, Twitch Ids, or Twitch Channel names
        /// </summary>
        /// <param name="ids">Array of values of the Ids, Twitch Ids, or Twitch Channel names of the requested Twitch Servers</param>
        /// <returns>A Twitch Servers Response</returns>
        public async virtual Task<ServersTwitchResponse> GetServersTwitchAsync(string[] ids, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            var sb = new StringBuilder();
            for (int i = 0; i < ids.Length; i++)
            {
                sb.Append(ids[i].Trim());

                if (i != ids.Length - 1)
                {
                    sb.Append(",");
                }
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/Servers/Twitch?ids={sb.ToString()}", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<ServersTwitchResponse>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Get a Discord Server by its Id or Discord Guild Id
        /// </summary>
        /// <param name="id">The Id or Discord Guild Id of the requested Discord Server</param>
        /// <returns>A Discord Server ViewModel</returns>
        public async virtual Task<ServerDiscordVM> GetServerDiscordAsync(string id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/Server/Discord/{id}", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<ServerDiscordVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Get Discord Servers by their Ids, Discord Ids, or Discord Channel names
        /// </summary>
        /// <param name="ids">Array of values of the Ids, Discord Ids, or Discord Channel names of the requested Discord Servers</param>
        /// <returns>A Discord Servers Response</returns>
        public async virtual Task<ServersDiscordResponse> GetServerDiscordAsync(string[] ids, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            var sb = new StringBuilder();
            for (int i = 0; i < ids.Length; i++)
            {
                sb.Append(ids[i].Trim());

                if (i != ids.Length - 1)
                {
                    sb.Append(",");
                }
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var server = await client.GetStringAsync($"{_webAPIBaseUrl}/Servers/Discord?ids={sb.ToString()}", cancellationToken);

                    if (!string.IsNullOrWhiteSpace(server))
                    {
                        return JsonConvert.DeserializeObject<ServersDiscordResponse>(server);
                    }
                }
            }
            catch
            { }

            return null;
        }

        /// <summary>
        /// Get the Users Online in a Twitch Server
        /// </summary>
        /// <param name="serverTwitchId">The Id of the Twitch Server to have its online Users retrieved</param>
        /// <returns>A Twitch Server Users Response</returns>
        public async Task<ServerUsersTwitchVM> GetServerTwitchUsersAsync(Guid serverTwitchId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/Servers/Twitch/Users/{serverTwitchId.ToString()}", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<ServerUsersTwitchVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Get the Users Online in a Discord Server
        /// </summary>
        /// <param name="serverDiscordId">The Id of the Discord Server to have its online Users retrieved</param>
        /// <returns>A Discord Server Users Response</returns>
        public async Task<ServerUsersDiscordVM> GetServerDiscordUsersAsync(Guid serverDiscordId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/Servers/Discord/Users/{serverDiscordId.ToString()}", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<ServerUsersDiscordVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }

        /// <summary>
        /// Get the Discord Channels in a Discord Server
        /// </summary>
        /// <param name="discordServerId">The Discord Server Id to retrieve the registered Channels</param>
        /// <returns>An array of Discord sServer Channel data-transfer objects</returns>
        public async virtual Task<ServerChannelDiscordDTO[]> GetChannelsDiscordAsync(Guid discordServerId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/Servers/Discord/Channels/{discordServerId}", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<ServerChannelDiscordDTO[]>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Get a Stream
        /// </summary>
        /// <param name="id">The Id of the requested Stream</param>
        /// <returns>A Stream ViewModel</returns>
        public async virtual Task<ServerChannelDiscordVM> GetChannelDiscordAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/Servers/Discord/Channel/{id.ToString()}", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<ServerChannelDiscordVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }

        /// <summary>
        /// Get the Users and their User Currencies in a Server
        /// </summary>
        /// <param name="serverId">The Id of the Server to retrieve the Users and their User Currencies</param>
        /// <returns>An array of Currencies User ViewModels</returns>
        public async virtual Task<CurrenciesUserVM[]> GetServerUsersCurrencies(Guid serverId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/Servers/Users/Currencies/{serverId.ToString()}", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<CurrenciesUserVM[]>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }

        /// <summary>
        /// Get the registered Streams
        /// </summary>
        /// <returns>An array of Stream data-transfer objects</returns>
        public async virtual Task<StreamDTO[]> GetStreamsAsync(CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/Streams", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<StreamDTO[]>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Get a Stream
        /// </summary>
        /// <param name="id">The Id of the requested Stream</param>
        /// <returns>A Stream ViewModel</returns>
        public async virtual Task<StreamVM> GetStreamAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/Streams/{id.ToString()}", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<StreamVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Create a Stream
        /// </summary>
        /// <param name="request">The Stream create request</param>
        /// <returns>A Stream ViewModel</returns>
        public async virtual Task<StreamVM> CreateStreamAsync(StreamCreateRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.PostAsync($"{_webAPIBaseUrl}/Streams", new JsonContent(request), cancellationToken);

                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        return JsonConvert.DeserializeObject<StreamVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Update a Stream
        /// </summary>
        /// <param name="request">The Stream update request</param>
        /// <returns>A Stream ViewModel</returns>
        public async virtual Task<StreamVM> UpdateStreamAsync(StreamUpdateRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.PutAsync($"{_webAPIBaseUrl}/Streams/{request.Id}", new JsonContent(request), cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<StreamVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Delete a Stream
        /// </summary>
        /// <param name="id">The Id of the Stream to be deleted</param>
        /// <returns>True if the delete was successful</returns>
        public async virtual Task<bool> DeleteStreamAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.DeleteAsync($"{_webAPIBaseUrl}/Streams/{id.ToString()}", cancellationToken);

                    return response.StatusCode == HttpStatusCode.NoContent;
                }
            }
            catch
            { }

            return false;
        }

        /// <summary>
        /// Get the Command Sets registered to a Stream
        /// </summary>
        /// <param name="streamId">The Id of the Stream where the requested Command Sets are registered</param>
        /// <returns>An array of Stream Command Set ViewModels</returns>
        public async virtual Task<StreamCommandSetVM[]> GetStreamCommandSetsAsync(Guid streamId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/Streams/CommandSets/{streamId}", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<StreamCommandSetVM[]>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Get a Stream Command Set
        /// </summary>
        /// <param name="id">The Id of the Stream Command Set</param>
        /// <returns>A Stream Command Set ViewModel</returns>
        public async virtual Task<StreamCommandSetVM> GetStreamCommandSetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/Streams/CommandSet/{id.ToString()}", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<StreamCommandSetVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Create a Stream Command Set
        /// </summary>
        /// <param name="request">A Stream Command Set create request</param>
        /// <returns>A Stream Command Set ViewModel</returns>
        public async virtual Task<StreamCommandSetVM> CreateStreamCommandSetAsync(StreamCommandSetCreateRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.PostAsync($"{_webAPIBaseUrl}/Streams/CommandSets", new JsonContent(request), cancellationToken);

                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        return JsonConvert.DeserializeObject<StreamCommandSetVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Delete a Stream Command Set
        /// </summary>
        /// <param name="id">The Id of the Stream Command Set to delete</param>
        /// <returns>True if the delete was successful</returns>
        public async virtual Task<bool> DeleteStreamCommandSetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.DeleteAsync($"{_webAPIBaseUrl}/Streams/CommandSets/{id.ToString()}", cancellationToken);

                    return response.StatusCode == HttpStatusCode.NoContent;
                }
            }
            catch
            { }

            return false;
        }

        /// <summary>
        /// Get the Stream Currencies registered to a Stream
        /// </summary>
        /// <param name="streamId">The Stream Id that the Stream Currencies are registered</param>
        /// <returns>An array of Stream Currency data-transfer objects</returns>
        public async virtual Task<StreamCurrencyDTO[]> GetStreamCurrenciesAsync(Guid streamId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/Streams/Currencies/{streamId}", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<StreamCurrencyDTO[]>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Get a Stream Currency
        /// </summary>
        /// <param name="id">The Id of the requested Stream Currency</param>
        /// <returns>A Stream Currency ViewModel</returns>
        public async virtual Task<StreamCurrencyVM> GetStreamCurrencyAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/Streams/Currencies/{id.ToString()}", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<StreamCurrencyVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Create a Stream Currency
        /// </summary>
        /// <param name="request">The Stream Currency create request</param>
        /// <returns>A Stream Currency ViewModel</returns>
        public async virtual Task<StreamCurrencyVM> CreateStreamCurrencyAsync(StreamCurrencyCreateRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.PostAsync($"{_webAPIBaseUrl}/Streams/Currencies", new JsonContent(request), cancellationToken);

                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        return JsonConvert.DeserializeObject<StreamCurrencyVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Update a Stream Currency
        /// </summary>
        /// <param name="request">The Stream Currency update request</param>
        /// <returns>A Stream Currency ViewModel</returns>
        public async virtual Task<StreamCurrencyVM> UpdateStreamCurrencyAsync(StreamCurrencyUpdateRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.PutAsync($"{_webAPIBaseUrl}/Streams/Currencies/{request.Id}", new JsonContent(request), cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<StreamCurrencyVM>(await response.Content.ReadAsStringAsync());
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Delete a Stream Currency
        /// </summary>
        /// <param name="id">The Id of the Stream Currency to be deleted</param>
        /// <returns>True if the delete was successful</returns>
        public async virtual Task<bool> DeleteStreamCurrencyAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.DeleteAsync($"{_webAPIBaseUrl}/Streams/Currencies/{id.ToString()}", cancellationToken);

                    return response.StatusCode == HttpStatusCode.NoContent;
                }
            }
            catch
            { }

            return false;
        }

        /// <summary>
        /// Get the Statuses registered to a Stream Currency
        /// </summary>
        /// <param name="streamCurrencyId">The Stream Currency Id that the Status are registered</param>
        /// <returns>An array of Status data-transfer objects</returns>
        public async virtual Task<StatusDTO[]> GetStreamCurrencyStatusesAsync(Guid streamCurrencyId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/Streams/Currencies/Statuses/{streamCurrencyId}", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<StatusDTO[]>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Get a Status
        /// </summary>
        /// <param name="id">The Id of the requested Status</param>
        /// <returns>A Status ViewModel</returns>
        public async virtual Task<StatusVM> GetStreamCurrencyStatusAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/Streams/Currencies/Status/{id.ToString()}", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<StatusVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Create a Status
        /// </summary>
        /// <param name="request">The Status create request</param>
        /// <returns>A Status ViewModel</returns>
        public async virtual Task<StatusVM> CreateStreamCurrencyStatusAsync(StatusCreateRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.PostAsync($"{_webAPIBaseUrl}/Streams/Currencies/Status", new JsonContent(request), cancellationToken);

                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        return JsonConvert.DeserializeObject<StatusVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Update a Status
        /// </summary>
        /// <param name="request">The Status update request</param>
        /// <returns>A Status ViewModel</returns>
        public async virtual Task<StatusVM> UpdateStreamCurrencyStatusAsync(StatusUpdateRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.PutAsync($"{_webAPIBaseUrl}/Streams/Currencies/Status/{request.Id}", new JsonContent(request), cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<StatusVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Delete a Status
        /// </summary>
        /// <param name="id">The Id of the Status to deleted</param>
        /// <returns>True if the delete was successful</returns>
        public async virtual Task<bool> DeleteStreamCurrencyStatusAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.DeleteAsync($"{_webAPIBaseUrl}/Streams/Currencies/Status/{id.ToString()}", cancellationToken);

                    return response.StatusCode == HttpStatusCode.NoContent;
                }
            }
            catch
            { }

            return false;
        }

        /// <summary>
        /// Get the Users online in a Stream
        /// </summary>
        /// <param name="streamId">The Stream Id where the Users are registered</param>
        /// <returns>A Stream Users ViewModel</returns>
        public async virtual Task<StreamUsersVM> GetStreamUsersAsync(Guid streamId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/Streams/Users/{streamId}", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<StreamUsersVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }

        /// <summary>
        /// Get the Users and their User Currencies in a Stream
        /// </summary>
        /// <param name="streamId">The Id of the Stream to retrieve the Users and their User Currencies</param>
        /// <returns>An array of Currencies User ViewModels</returns>
        public async virtual Task<CurrenciesUserVM[]> GetStreamUsersCurrencies(Guid streamId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/Streams/Users/Currencies/{streamId.ToString()}", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<CurrenciesUserVM[]>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }

        /// <summary>
        /// Get the Users registered to the Application User
        /// </summary>
        /// <returns>An array of User data-transfer objects</returns>
        public async Task<UserDTO[]> GetUsersAsync(CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/Users", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<UserDTO[]>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Get multiple Users by their Ids
        /// </summary>
        /// <param name="ids">Ids of the requested Users</param>
        /// <returns>A Users Response</returns>
        public async Task<UsersResponse> GetUsersAsync(Guid[] ids, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var sb = new StringBuilder();
                    for (int i = 0; i < ids.Length; i++)
                    {
                        sb.Append(ids[i].ToString());

                        if (i != ids.Length - 1)
                        {
                            sb.Append(",");
                        }
                    }

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/Users?ids={sb.ToString()}", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<UsersResponse>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Get a Twitch User by their Id, Twitch Id, or Twitch Username
        /// </summary>
        /// <param name="id">The Id, Twitch Id, or Twitch Username of the requested User</param>
        /// <returns>A Twitch User ViewModel</returns>
        public async Task<UserTwitchVM> GetUserTwitchAsync(string id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/User/Twitch/{id}", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<UserTwitchVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Get multiple Twitch Users by their Ids or Twitch Ids
        /// </summary>
        /// <param name="ids">Ids or Twitch Ids of the requested Twitch Users</param>
        /// <returns>A Users Twitch Response</returns>
        public async Task<UsersTwitchResponse> GetUsersTwitchAsync(string[] ids, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var sb = new StringBuilder();
                    for (int i = 0; i < ids.Length; i++)
                    {
                        sb.Append(ids[i].ToString());

                        if (i != ids.Length - 1)
                        {
                            sb.Append(",");
                        }
                    }

                    var response = await client.GetStringAsync($"{_webAPIBaseUrl}/Users/Twitch?ids={sb.ToString()}", cancellationToken);

                    if (!string.IsNullOrWhiteSpace(response))
                    {
                        return JsonConvert.DeserializeObject<UsersTwitchResponse>(response);
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Get a Discord User by their Id or Discord Id
        /// </summary>
        /// <param name="id">The Id or Discord Id of the requested User</param>
        /// <returns>A Discord User ViewModel</returns>
        public async Task<UserDiscordVM> GetUserDiscordAsync(string id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var response = await client.GetAsync($"{_webAPIBaseUrl}/User/Discord/{id}", cancellationToken);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<UserDiscordVM>(await response.Content.ReadAsStringAsync(cancellationToken));
                    }
                }
            }
            catch
            { }

            return null;
        }
        /// <summary>
        /// Get multiple Discord Users by their Ids or Discord Ids
        /// </summary>
        /// <param name="ids">The Ids or Discord Ids of the requested Users</param>
        /// <returns>A Users Discord Response</returns>
        public async Task<UsersDiscordResponse> GetUsersDiscordAsync(string[] ids, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new Exception("There is no access token currently loaded to access the WebAPI. Please load a new access token and try again");
            }

            try
            {
                using (var client = _httpClientFactory != null ? _httpClientFactory.CreateClient() : new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    var sb = new StringBuilder();
                    for (int i = 0; i < ids.Length; i++)
                    {
                        sb.Append(ids[i].ToString());

                        if (i != ids.Length - 1)
                        {
                            sb.Append(",");
                        }
                    }

                    var response = await client.GetStringAsync($"{_webAPIBaseUrl}/Users/Discord?ids={sb.ToString()}", cancellationToken);

                    if (!string.IsNullOrWhiteSpace(response))
                    {
                        return JsonConvert.DeserializeObject<UsersDiscordResponse>(response);
                    }
                }
            }
            catch
            { }

            return null;
        }
        
        public virtual void Dispose()
        {
        }

        public class JsonContent : StringContent
        {
            public JsonContent(object obj) : base(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json")
            { }
        }
    }
}
