using System;
using System.Threading.Tasks;

namespace Allie.Chat.Commands.Service.Services
{
    public interface IBasePollingService : IDisposable
    {
        void Update(int updateIntervalMS);
    }
}