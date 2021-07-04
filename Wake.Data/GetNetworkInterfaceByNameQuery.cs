using MediatR;
using Wake.Shared;

namespace Wake.Data
{
    public class GetNetworkInterfaceByNameQuery: IRequest<Result<NetworkInterface>>
    {
        public string Name { get; }

        public GetNetworkInterfaceByNameQuery(string name)
        {
            Name = name;
        }
    }
}