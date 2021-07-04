using MediatR;
using Wake.Shared;

namespace Wake.Data
{
    public class InsertNetworkInterfaceCommand : IRequest<Result<NetworkInterface>>
    {
        public string Name { get; }
        public string MacAddress { get; }

        public InsertNetworkInterfaceCommand(string name, string macAddress)
        {
            Name = name;
            MacAddress = macAddress;
        }
    }
}