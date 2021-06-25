using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace WakeOnLan
{
    public class Wake: BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var macAddress= "1C-1B-0D-EC-CC-98";
            var target = PhysicalAddress.Parse(macAddress);
            var header = Enumerable.Repeat(byte.MaxValue, 6);
            var data = Enumerable.Repeat(target.GetAddressBytes(), 16).SelectMany(mac => mac);

            var magicPacket = header.Concat(data).ToArray();

            using var client = new UdpClient();

            await client.SendAsync(magicPacket, magicPacket.Length, new IPEndPoint(IPAddress.Broadcast, 9));

            throw new OperationCanceledException();
        }
    }
}