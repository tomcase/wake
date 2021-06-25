using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WakeOnLan
{
    class Program
    {
        private static async Task Main(string[] args)
        {
            try
            {
                using IHost host = CreateHostBuilder(args).Build();
                var dbPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                await host.RunAsync();
            }
            catch (OperationCanceledException)
            {
                // Meh
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(ConfigureServices);
        

        private static void ConfigureServices(HostBuilderContext builder, IServiceCollection collection)
        {
            collection.AddHostedService<Wake>();
        }
    }
}