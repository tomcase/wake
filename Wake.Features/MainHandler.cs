using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Wake.Data;
using Wake.Shared;
using Wake.Shared.Writers;

namespace Wake.Features
{
    public class MainHandler : IRequestHandler<RunMainCommand, Result>
    {
        private readonly IMediator _mediator;
        private readonly IConsoleInteractive _console;

        public MainHandler(IMediator mediator, IConsoleInteractive console)
        {
            _mediator = mediator;
            _console = console;
        }

        public async Task<Result> Handle(RunMainCommand request, CancellationToken cancellationToken)
        {
            var send = await _mediator.Send(new ListNetworkInterfacesQuery(), cancellationToken);
            if (send.IsFailure)
                return Result.Fail("Failed to retrieve list of Network Interfaces from the db.");

            if (send.Value.Any())
            {
                _console.WriteLine("Saved NICs:");
                foreach (var nic in send.Value)
                {
                    _console.WriteLine($"{nic.Name}: {nic.MacAddress}");
                }
            }

            _console.WriteLine("What's the MAC address of the NIC you want to wake?");
            var macAddress = _console.ReadLine();

            var existingMac = await _mediator.Send(new GetNetworkInterfaceByMacAddressQuery(macAddress), cancellationToken);
            if (existingMac.IsFailure)
                return Result.Fail(existingMac.Error);
            if (existingMac.Value is null)
            {
                var needToSave = GetYesOrNo("Do you want to save this MAC address for later? [Y/N]");
                if (needToSave)
                {
                    _console.WriteLine("Give it a name.");
                    var name = _console.ReadLine();
                    var result = await _mediator.Send(new InsertNetworkInterfaceCommand(name, macAddress),
                        cancellationToken);
                    if (result.IsFailure)
                        return Result.Fail<Result>(result.Error);
                }
            }

            _console.WriteLine($"Waking {macAddress}...");
            var wakeOnLanCommand = new WakeOnLanCommand(macAddress);
            return await _mediator.Send(wakeOnLanCommand, cancellationToken);
        }

        private bool GetYesOrNo(string question)
        {
            bool? result = null;
            _console.WriteLine(question);
            while (!result.HasValue)
            {
                var consoleKeyInfo = _console.ReadKey();
                switch (consoleKeyInfo.Key)
                {
                    case ConsoleKey.Y:
                        result = true;
                        break;
                    case ConsoleKey.N:
                        result = false;
                        break;
                    default:
                        _console.WriteLine("Please choose Y or N.");
                        _console.WriteLine(question);
                        break;
                }
            }

            return result.Value;
        }
    }
}