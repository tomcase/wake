using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Wake.Data;
using Wake.EF;
using Wake.Shared.Writers;

namespace Wake.Cmd
{
    static class Program
    {
        private static async Task Main(string[] args)
        {
            try
            {
                var provider = BuildServiceProvider();

                await MigrateDb(provider);
                await RunMainRoutine(provider, args);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Cancellation Requested.");
            }
        }

        private static async Task RunMainRoutine(ServiceProvider provider, string[] args)
        {
            var mediator = provider.GetRequiredService<IMediator>();
            var console = provider.GetRequiredService<IConsoleInteractive>();
            if (args.Length == 0)
            {
                var cts = new CancellationTokenSource();
                var startCmd = new RunMainCommand();
                var result = await mediator.Send(startCmd, cts.Token);
                if (result.IsFailure)
                    console.WriteLine($"Error: {result.Error}");
            }
            else
            {
                var firstArg = args[0];
                var result = await mediator.Send(new GetNetworkInterfaceByNameQuery(firstArg));
                if (result.IsFailure)
                    console.WriteLine($"Error: {result.Error}");
                if (result.Value is null)
                {
                    result = await mediator.Send(new GetNetworkInterfaceByMacAddressQuery(firstArg));
                    if (result.IsFailure)
                        console.WriteLine($"Error: {result.Error}");
                }

                if (result.Value is null)
                {
                    console.WriteLine($"Waking {firstArg}...");
                    await mediator.Send(new WakeOnLanCommand(firstArg));
                }
                else
                {
                    console.WriteLine($"Waking {result.Value.MacAddress}...");
                    await mediator.Send(new WakeOnLanCommand(result.Value.MacAddress));
                }
            }
        }

        private static async Task MigrateDb(ServiceProvider provider)
        {
            var context = provider.GetRequiredService<WakeContext>();
            await context.Database.MigrateAsync();
        }

        private static ServiceProvider BuildServiceProvider()
        {
            var services = new ServiceCollection();

            services.AddDbContext<WakeContext>();
            services.AddSingleton<IConsoleInteractive, ConsoleInteractive>();

            var assemblies = new[]
            {
                Assembly.Load("Wake.Cmd"),
                Assembly.Load("Wake.Data"),
                Assembly.Load("Wake.EF"),
                Assembly.Load("Wake.Features")
            };
            services.AddMediatR(assemblies);
            services.AddAutoMapper(assemblies);

            return services.BuildServiceProvider();
        }
    }
}