using AutoMapper;
using Wake.Data;

namespace Wake.EF
{
    public class NetworkInterfaceMapping : Profile
    {
        public NetworkInterfaceMapping()
        {
            CreateMap<Data.NetworkInterface, NetworkInterface>()
                .ReverseMap();

            CreateMap<InsertNetworkInterfaceCommand, NetworkInterface>()
                .ForMember(dest => dest.Id, x => x.Ignore());
        }
    }
}