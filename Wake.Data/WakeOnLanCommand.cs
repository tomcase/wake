using MediatR;
using Wake.Shared;

namespace Wake.Data
{
    public class WakeOnLanCommand: IRequest<Result>
    {
        public string MacAddress { get; }

        public WakeOnLanCommand(string macAddress)
        {
            MacAddress = macAddress;
        }
    }
}