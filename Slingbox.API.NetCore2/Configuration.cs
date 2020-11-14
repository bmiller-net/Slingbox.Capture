using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Slingbox.API.NetCore2
{
    public class SlingboxConfiguration
    {
        public Slingbox Slingbox { get; set; }
    }

    public class Slingbox
    {
        public string IPAddress { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string AdminPassword { get; set; }
    }
}
