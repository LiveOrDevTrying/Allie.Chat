using System;
using System.Threading.Tasks;

namespace Allie.Chat.Services
{
    public interface IBasePollingService : IDisposable
    {
        void Update(int updateIntervalMS);
    }
}