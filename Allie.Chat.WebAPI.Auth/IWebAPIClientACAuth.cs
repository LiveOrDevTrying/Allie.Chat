using IdentityModel.Client;
using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Results;
using System.Threading.Tasks;

namespace Allie.Chat.WebAPI.Auth
{
    public interface IWebAPIClientACAuth : IWebAPIClientAC
    {
        Task<LoginResult> GetAccessTokenAuthCodeAsync(string clientId, string clientSecret, string scopes);
        Task<LoginResult> GetAccessTokenNativePKCEAsync(string clientId, string scopes);
        Task<TokenResponse> GetAccessTokenResourceOwnerPasswordAsync(string clientId, string clientSecret,
            string scopes, string username, string password);

        Task<RefreshTokenResult> RefreshAccessTokenAuthCodeOrNativeAsync(string refreshToken);
        Task<TokenResponse> RefreshAccessTokenResourceOwnerPasswordAsync(string clientId,
            string clientSecret, string refreshToken);

        Task<UserInfoResult> GetUserInfoAuthCodeOrNativeAsync();
        Task<UserInfoResponse> GetUserInfoResourceOwnerPasswordAsync();

        Task<IntrospectionResponse> IntrospectAccessTokenAsync(string clientId, string clientSecret, string apiName, string apiSecret);
    }
}