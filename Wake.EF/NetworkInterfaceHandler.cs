using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Wake.Data;
using Wake.Shared;

namespace Wake.EF
{
    public class NetworkInterfaceHandler: 
        IRequestHandler<InsertNetworkInterfaceCommand, Result<Wake.Data.NetworkInterface>>,
        IRequestHandler<ListNetworkInterfacesQuery, Result<IEnumerable<Wake.Data.NetworkInterface>>>,
        IRequestHandler<GetNetworkInterfaceByMacAddressQuery, Result<Wake.Data.NetworkInterface>>,
        IRequestHandler<GetNetworkInterfaceByNameQuery, Result<Wake.Data.NetworkInterface>>
    {
        private readonly WakeContext _ctx;
        private readonly IMapper _mapper;

        public NetworkInterfaceHandler(WakeContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }
        
        public async Task<Result<Data.NetworkInterface>> Handle(InsertNetworkInterfaceCommand request, CancellationToken cancellationToken)
        {
            var networkInterface = _mapper.Map<NetworkInterface>(request);
            await _ctx.NetworkInterfaces.AddAsync(networkInterface, cancellationToken);
            await _ctx.SaveChangesAsync(cancellationToken);
            var result = _mapper.Map<Data.NetworkInterface>(networkInterface);
            return Result.Ok(result);
        }

        public async Task<Result<IEnumerable<Data.NetworkInterface>>> Handle(ListNetworkInterfacesQuery request, CancellationToken cancellationToken)
        {
            var networkInterfaces = await _ctx.NetworkInterfaces.ToListAsync(cancellationToken);
            var interfaces = networkInterfaces.Select(x => _mapper.Map<Data.NetworkInterface>(x));
            return Result.Ok(interfaces);
        }

        public async Task<Result<Data.NetworkInterface>> Handle(GetNetworkInterfaceByMacAddressQuery request, CancellationToken cancellationToken)
        {
            var nic = await _ctx.NetworkInterfaces.FirstOrDefaultAsync(x => x.MacAddress == request.MacAddress, cancellationToken);
            return nic is null ? Result.Ok<Data.NetworkInterface>(null) : Result.Ok(_mapper.Map<Data.NetworkInterface>(nic));
        }

        public async Task<Result<Data.NetworkInterface>> Handle(GetNetworkInterfaceByNameQuery request, CancellationToken cancellationToken)
        {
            var nic = await _ctx.NetworkInterfaces.FirstOrDefaultAsync(x => x.Name == request.Name, cancellationToken);
            return nic is null ? Result.Ok<Data.NetworkInterface>(null) : Result.Ok(_mapper.Map<Data.NetworkInterface>(nic));
        }
    }
}