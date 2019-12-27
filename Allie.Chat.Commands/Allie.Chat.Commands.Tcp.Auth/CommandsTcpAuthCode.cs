using Allie.Chat.Commands.Tcp.Auth.Interfaces;
using IdentityModel.OidcClient;
using System;
using System.Threading.Tasks;

namespace Allie.Chat.Commands.Tcp.Auth
{
    public class CommandsTcpAuthCode : BaseCommandsTcpAuth
    {
        protected new IParametersTcpAuthCode _parameters;
        protected LoginResult _loginResult;
        protected DateTime _expireTime;

        public CommandsTcpAuthCode(IParametersTcpAuthCode parameters) 
            : base(parameters)
        {
        }

        protected override async Task GetAccessTokenAsync()
        {
            // R8D: Do this with a refresh token
            _loginResult = await _webapiClient.GetAccessTokenAuthCodeAsync(_parameters.ClientId, _parameters.ClientSecret, _parameters.Scopes);

            if (_loginResult != null)
            {
                if (!string.IsNullOrWhiteSpace(_loginResult.AccessToken))
                {
                    UpdateWebAPIToken(_loginResult.AccessToken);
                    Connect();
                }
            }
        }
        protected override async Task<bool> IsTokenValidAsync()
        {
            if (_loginResult.AccessTokenExpiration <= DateTime.Now)
            {
                await GetAccessTokenAsync();
            }

            return await base.IsTokenValidAsync();
        }
    }
}
