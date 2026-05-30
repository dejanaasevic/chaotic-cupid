using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaoticCupid.Server.Models
{
    public class Letter
    {
        public string FromUsername { get; set; }
        public string FromCity { get; set; }
        public int FromAge { get; set; }
        public string FromPhone { get; set; }
        public string Message { get; set; }
        public bool ShowPhone { get; set; }
    }
}