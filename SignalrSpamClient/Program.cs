using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace SignalrSpamClient
{
    internal class Program
    {
        static long prevCounter;

        static async Task Main(string[] args)
        {
            var log = LoggerFactory.Create(x => x.AddConsole()).CreateLogger<Program>();

            log.LogInformation("Waiting");

            await Task.Delay(4000); // give it some startup time

            var connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7170/signalr/test")
                .ConfigureLogging(x => x.AddConsole())
                .Build();


            using var subscription = connection.On<long>("counter", (counter) =>
            {
                if (counter < prevCounter)
                {
                    log.LogWarning($"Oudated {counter}, prev was {prevCounter}");
                }
                else
                {
                    prevCounter = counter;
                }

                if (counter % 100 == 0)
                    log.LogInformation($"Counter at {counter}");
            });

            await connection.StartAsync();

            log.LogInformation("Connected, press enter to stop");

            Console.ReadLine();

            await connection.StopAsync();
        }
    }
}