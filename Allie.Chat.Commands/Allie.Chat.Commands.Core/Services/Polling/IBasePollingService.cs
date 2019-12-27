using System;
using System.Threading.Tasks;

namespace Allie.Chat.Commands.Core.Services
{
    public interface IBasePollingService : IDisposable
    {
        void Update(int updateIntervalMS);
    }
}