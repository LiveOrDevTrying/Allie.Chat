﻿using Allie.Chat.Commands.Core.Auth.Interfaces;
using Allie.Chat.WebAPI.Auth;
using IdentityModel.OidcClient;
using System;
using System.Threading.Tasks;

namespace Allie.Chat.Commands.Tcp.Auth
{
    public class CommandsTcpAuthPKCE : BaseCommandsTcpAuth
    {
        protected new readonly IParametersAuthPKCE _parameters;
        protected LoginResult _loginResult;
        protected DateTime _expireTime;

        public CommandsTcpAuthPKCE(IParametersAuthPKCE parameters) 
            : base(parameters)
        {
            _parameters = parameters;
        }

        protected override async Task GetAccessTokenAsync()
        {
            // R8D: Do this with a refresh token
            _loginResult = await _webapiClient.GetAccessTokenNativePKCEAsync(_parameters.ClientId, _parameters.Scopes);

            if (_loginResult != null)
            {
                if (!string.IsNullOrWhiteSpace(_loginResult.AccessToken))
                {
                    UpdateWebAPIToken(_loginResult.AccessToken);
                    await UpdateEventsAsync(0);
                    Connect();
                }
            }
        }
        protected override async Task ValidateTokenAsync()
        {
            if (_loginResult.AccessTokenExpiration <= DateTime.Now)
            {
                await GetAccessTokenAsync();
            };
        }
    }
}
