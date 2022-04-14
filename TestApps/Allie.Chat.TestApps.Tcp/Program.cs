using Allie.Chat.Events.Args;
using Allie.Chat.Models;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Allie.Chat.TestApps.Tcp
{
    class Program
    {
        private static Timer _timer;
        private static ACCommands _acCommands;

        static void Main(string[] args)
        {
            _acCommands = new ACCommands(new ACParametersAuthCode
            {
                BotAccessToken = "ad4b22edf15240d29aa77a3246f754f77cba2f0d49b0403f939439ab7b497c82",
                ClientId = "auth.code",
                ClientSecret = "secret",
                Scopes = "openid profile Allie.Chat.WebAPI",
                StreamCachePollingIntervalMS = 45000,
                ReconnectPollingIntervalMS = 15000
            });
            _acCommands.CommandEvent += OnCommandEvent;
            Task.Run(async () => await _acCommands.StartAsync());

            _timer = new Timer(TimerCallback, null, 0, 1000);
            while (true)
            {
                Console.ReadLine();
            }
        }

        private static void OnCommandEvent(object sender, CommandEventArgs args)
        {
            Console.WriteLine(JsonConvert.SerializeObject(args));
        }

        private static void TimerCallback(object state)
        {
            _acCommands.Update(1000);
        }
    }
}
