using DAL.Core.Interfaces;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BLL.Core
{
	public class EventStatusTransferService : IHostedService
    {
        private readonly IEventRepository _eventRepository;
        private Timer _timer;

        public EventStatusTransferService(IEventRepository eventRepository)
		{
            _eventRepository = eventRepository;
		}

        private void DoWork(object state)
        {
            _eventRepository.TransferEventsStatuses().Wait();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(60));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
    }
}
