using Microsoft.AspNetCore.SignalR;

namespace SignalrSpamServer
{
    public class SpamService : IHostedService
    {
        private readonly IHubContext<TestHub> hubContext;
        private readonly ILogger<SpamService> log;

        public SpamService(
            IHubContext<TestHub> hubContext,
            ILogger<SpamService> log)
        {
            this.hubContext = hubContext;
            this.log = log;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() => SpamTask(cancellationToken), TaskCreationOptions.LongRunning);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private async Task SpamTask(CancellationToken cancellationToken)
        {
            long counter = 0;

            log.LogInformation("Waiting");

            await Task.Delay(3000);

            log.LogInformation("Spamming");

            while (!cancellationToken.IsCancellationRequested)
            {
                counter++;

                await hubContext.Clients.All.SendAsync("counter", counter);

                await Task.Delay(1);
            }
        }
    }
}
