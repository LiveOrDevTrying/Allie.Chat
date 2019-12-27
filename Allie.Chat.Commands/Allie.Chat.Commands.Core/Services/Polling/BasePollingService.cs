using System.Threading.Tasks;

namespace Allie.Chat.Commands.Core.Services
{
    public abstract class BasePollingService : IBasePollingService
    {
        protected int _pollingIndex;
        protected bool _isPollRunning;
        protected int _pollingIntervalMS;

        public BasePollingService(int pollingIntervalMS)
        {
            _pollingIntervalMS = pollingIntervalMS;
        }

        public virtual void Update(int updateIntervalMS)
        {
            if (!_isPollRunning)
            {
                _pollingIndex += updateIntervalMS;

                if (_pollingIndex >= _pollingIntervalMS)
                {
                    _isPollRunning = true;
                    _pollingIndex = 0;

                    Task.Run(async () =>
                    {
                        try
                        {
                            UpdateEvents(updateIntervalMS);
                        }
                        catch
                        { }

                        try
                        {
                            await UpdateEventsAsync(updateIntervalMS);
                        }
                        catch
                        { }

                        _isPollRunning = false;
                    });
                }
            }
        }

        protected abstract void UpdateEvents(int updateIntervalMS);
        protected abstract Task UpdateEventsAsync(int updateIntervalMS);

        public virtual void Dispose() { }
    }
}
