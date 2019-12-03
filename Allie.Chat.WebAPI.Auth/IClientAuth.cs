using IdentityModel.Client;
using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Results;
using System.Threading.Tasks;

namespace Allie.Chat.WebAPI.Auth
{
    public interface IClientAuth : IClient
    {
        Task<LoginResult> GetAccessTokenAuthCodeAsync(string identityServerAuthorityUrl, string clientId, string clientSecret, string scopes);
        Task<LoginResult> GetAccessTokenNativePKCEAsync(string identityServerAuthorityUrl, string clientId, string scopes);
        Task<TokenResponse> GetAccessTokenResourceOwnerPasswordAsync(string identityServerAuthorityUrl, string clientId, string clientSecret,
            string scopes, string username, string password);

        Task<RefreshTokenResult> RefreshAccessTokenAuthCodeOrNativeAsync(string refreshToken);
        Task<TokenResponse> RefreshAccessTokenResourceOwnerPasswordAsync(string identityServerAuthorityUrl, string clientId,
            string clientSecret, string refreshToken);

        Task<UserInfoResult> GetUserInfoAuthCodeOrNativeAsync();
        Task<UserInfoResponse> GetUserInfoResourceOwnerPasswordAsync(string identityServerAuthorityUrl);

        Task<IntrospectionResponse> IntrospectAccessTokenAsync(string identityServerAuthorityUrl, string clientId, string clientSecret, string apiName, string apiSecret);
    }
}