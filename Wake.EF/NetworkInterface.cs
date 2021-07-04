using System;

namespace Wake.EF
{
    public class NetworkInterface
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string MacAddress { get; set; }
    }
}