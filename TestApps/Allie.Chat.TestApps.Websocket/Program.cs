﻿using Allie.Chat.Commands.Core.Auth.Models;
using Allie.Chat.Commands.Core.Events.Args;
using Allie.Chat.Commands.Websocket.Auth;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Allie.Chat.TestApps.Websocket
{
    class Program
    {
        private static CommandsWebsocketAuthCode _commandsAuthCode;

        static void Main(string[] args)
        {
            _commandsAuthCode = new CommandsWebsocketAuthCode(new ParametersAuthCode
            {
                BotAccessToken = "c88cbdcbce25497d8ba9edb71a97f06cd3e1fda419bc4adfb42cfc283f0bb29d",
                ClientId = "auth.code",
                ClientSecret = "secret",
                Scopes = "openid profile Allie.Chat.WebAPI",
                StreamCachePollingIntervalMS = 45000,
                ReconnectPollingIntervalMS = 15000
            });
            _commandsAuthCode.CommandEvent += OnCommandEvent;

            while (true)
            {
                Console.ReadLine();
            }
        }

        private static Task OnCommandEvent(object sender, CommandEventArgs args)
        {
            switch (args.Command.CommandText.Trim().ToLower())
            {
                case "time":
                    _commandsAuthCode.SendMessageAsync($"{DateTime.Now.ToLocalTime().ToShortDateString()} {DateTime.Now.ToLocalTime().ToShortTimeString()}");
                    break;
                default:
                    break;
            }


            Console.WriteLine(JsonConvert.SerializeObject(args));
            return Task.CompletedTask;
        }
    }
}