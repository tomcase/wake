using MediatR;
using Wake.Shared;

namespace Wake.Data
{
    public class GetNetworkInterfaceByMacAddressQuery: IRequest<Result<NetworkInterface>>
    {
        public GetNetworkInterfaceByMacAddressQuery(string macAddress)
        {
            MacAddress = macAddress;
        }

        public string MacAddress { get; }
    }
}