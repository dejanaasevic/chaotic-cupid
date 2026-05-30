using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaoticCupid.Server.Models
{
    public class Person
    {
        public string ConnectionId { get; set; }
        public string Username { get; set; }
        public string City { get; set; }
        public int Age { get; set; }
        public string PhoneNumber { get; set; }
        public List<string> BlockedUsers { get; set; } = new List<string>();
        public bool IsWaitingConfirmation { get; set; }    
    }
}