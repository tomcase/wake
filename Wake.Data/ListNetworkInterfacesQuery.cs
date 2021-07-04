using System.Collections.Generic;
using MediatR;
using Wake.Shared;

namespace Wake.Data
{
    public class ListNetworkInterfacesQuery: IRequest<Result<IEnumerable<NetworkInterface>>>
    {
    }
}