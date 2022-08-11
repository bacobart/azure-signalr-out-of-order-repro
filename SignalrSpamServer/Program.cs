using Newtonsoft.Json.Serialization;

namespace SignalrSpamServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Logging.AddConsole();

            builder.Services
                .AddSignalR();
                // without .AddAzureSignalR messages are received in order (i.e. no "Oudated" warning messages logged on client) as expected
                // however if you uncomment the below line then every 100 messages or so there are a few received out of order.
                //.AddAzureSignalR("Endpoint=https://use-your-azure-signalr.service.signalr.net;AccessKey=insert-access-key-here;Version=1.0;");

            builder.Services.AddHostedService<SpamService>();

            var app = builder.Build();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<TestHub>("/signalr/test");
            });

            app.Run();
        }
    }
}