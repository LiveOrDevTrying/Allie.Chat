using IdentityModel.Client;
using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Results;
using System.Net.Http;
using System.Threading.Tasks;

namespace Allie.Chat.WebAPI.Auth
{
    public class WebAPIClientACAuth : WebAPIClientAC, IWebAPIClientACAuth
    {
        protected OidcClient _oidcClient;

        public WebAPIClientACAuth() : base(string.Empty)
        {
        }

        public async Task<TokenResponse> GetAccessTokenResourceOwnerPasswordAsync(string identityServerAuthorityUrl, string clientId, string clientSecret,
            string scopes, string username, string password)
        {
            using (var client = new HttpClient())
            {
                var disco = await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
                {
                    Address = identityServerAuthorityUrl,
                });

                var response = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
                {
                    Address = disco.TokenEndpoint,
                    ClientId = clientId,
                    ClientSecret = clientSecret,
                    UserName = username,
                    Password = password,
                    Scope = scopes,
                });

                if (response != null)
                {
                    _accessToken = response.AccessToken;
                }

                return response;
            }
        }
        public async Task<LoginResult> GetAccessTokenAuthCodeAsync(string identityServerAuthorityUrl, string clientId, string clientSecret, string scopes)
        {
            // create a redirect URI using an available port on the loopback address.
            // requires the OP to allow random ports on 127.0.0.1 - otherwise set a static port
            var browser = new SystemBrowser(45656);
            string redirectUri = string.Format($"http://127.0.0.1:{browser.Port}");

            var options = new OidcClientOptions
            {
                Authority = identityServerAuthorityUrl,
                ClientId = clientId,
                RedirectUri = redirectUri,
                Scope = scopes,
                FilterClaims = false,
                Browser = browser,
                RefreshTokenInnerHttpHandler = new HttpClientHandler(),
                Flow = OidcClientOptions.AuthenticationFlow.AuthorizationCode,
                ClientSecret = clientSecret
            };

            _oidcClient = new OidcClient(options);
            var result = await _oidcClient.LoginAsync(new LoginRequest());

            if (result != null)
            {
                _accessToken = result.AccessToken;
            }

            return result;
        }
        public async Task<LoginResult> GetAccessTokenNativePKCEAsync(string identityServerAuthorityUrl, string clientId, string scopes)
        {
            // create a redirect URI using an available port on the loopback address.
            // requires the OP to allow random ports on 127.0.0.1 - otherwise set a static port
            var browser = new SystemBrowser(45656);
            string redirectUri = string.Format($"http://127.0.0.1:{browser.Port}");

            var options = new OidcClientOptions
            {
                Authority = identityServerAuthorityUrl,
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

            if (result != null)
            {
                _accessToken = result.AccessToken;
            }

            return result;
        }

        public async Task<RefreshTokenResult> RefreshAccessTokenAuthCodeOrNativeAsync(string refreshToken)
        {
            if (_oidcClient != null)
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
        public async Task<TokenResponse> RefreshAccessTokenResourceOwnerPasswordAsync(string identityServerAuthorityUrl, string clientId,
            string clientSecret, string refreshToken)
        {
            using (var client = new HttpClient())
            {
                var disco = await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
                {
                    Address = identityServerAuthorityUrl,
                });

                var result = await client.RequestRefreshTokenAsync(new RefreshTokenRequest
                {
                    Address = disco.TokenEndpoint,
                    RefreshToken = refreshToken,
                    ClientId = clientId,
                    ClientSecret = clientSecret
                });

                if (result != null)
                {
                    _accessToken = result.AccessToken;
                }

                return result;
            }
        }

        public async Task<UserInfoResult> GetUserInfoAuthCodeOrNativeAsync()
        {
            return _oidcClient != null ? await _oidcClient.GetUserInfoAsync(_accessToken) : null;
        }
        public async Task<UserInfoResponse> GetUserInfoResourceOwnerPasswordAsync(string identityServerAuthorityUrl)
        {
            using (var client = new HttpClient())
            {
                var disco = await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
                {
                    Address = identityServerAuthorityUrl,
                });

                return await client.GetUserInfoAsync(new UserInfoRequest
                {
                    Address = disco.UserInfoEndpoint,
                    Token = _accessToken
                });
            }
        }

        public async Task<IntrospectionResponse> IntrospectAccessTokenAsync(string identityServerAuthorityUrl, string clientId, string clientSecret, string apiName, string apiSecret)
        {
            using (var client = new HttpClient())
            {
                var disco = await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
                {
                    Address = identityServerAuthorityUrl,
                });

                client.SetBasicAuthentication(apiName, apiSecret);

                return await client.IntrospectTokenAsync(new TokenIntrospectionRequest
                {
                    Address = disco.IntrospectionEndpoint,
                    ClientId = clientId,
                    ClientSecret = clientSecret,
                    Token = _accessToken,
                });
            }
        }
    }
}
