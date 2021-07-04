using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Wake.Data;
using Wake.Shared;
using Wake.Shared.Writers;

namespace Wake.Features
{
    public class WakeHandler: IRequestHandler<WakeOnLanCommand, Result>
    {
        private readonly IConsoleInteractive _console;

        public WakeHandler(IConsoleInteractive console)
        {
            _console = console;
        }
        
        public async Task<Result> Handle(WakeOnLanCommand notification, CancellationToken cancellationToken)
        {
            try
            {
                var target = PhysicalAddress.Parse(notification.MacAddress);
                var header = Enumerable.Repeat(byte.MaxValue, 6);
                var data = Enumerable.Repeat(target.GetAddressBytes(), 16).SelectMany(mac => mac);

                var magicPacket = header.Concat(data).ToArray();

                using var client = new UdpClient();

                await client.SendAsync(magicPacket, magicPacket.Length, new IPEndPoint(IPAddress.Broadcast, 9));

                return Result.Ok();
            }
            catch (FormatException)
            {
                var message = "Malformed MAC address.";
                _console.WriteLine(message);
                return Result.Fail(message);
            }
        }
    }
}
