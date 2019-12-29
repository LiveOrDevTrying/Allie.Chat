﻿using Allie.Chat.Commands.Core.Auth.Interfaces;
using Allie.Chat.WebAPI.Auth;
using IdentityModel.OidcClient;
using System;
using System.Threading.Tasks;

namespace Allie.Chat.Commands.Tcp.Auth
{
    public class CommandsTcpAuthCode : BaseCommandsTcpAuth
    {
        protected new IParametersAuthCode _parameters;
        protected LoginResult _loginResult;
        protected DateTime _expireTime;

        public CommandsTcpAuthCode(IParametersAuthCode parameters) 
            : base(parameters)
        {
            _parameters = parameters;
        }

        protected override async Task GetAccessTokenAsync()
        {
            // R8D: Do this with a refresh token
            _loginResult = await _webapiClient.GetAccessTokenAuthCodeAsync(_parameters.ClientId, _parameters.ClientSecret, _parameters.Scopes);

            if (_loginResult != null)
            {
                UpdateWebAPIToken(_loginResult.AccessToken);
                await UpdateEventsAsync(0);
                Connect();
            }
        }
        protected override async Task ValidateTokenAsync()
        {
            if (_loginResult.AccessTokenExpiration <= DateTime.Now)
            {
                await GetAccessTokenAsync();
            }
        }
    }
}
