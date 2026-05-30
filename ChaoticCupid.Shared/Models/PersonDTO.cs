using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaoticCupid.Shared.Models
{
    public class PersonDTO
    {
        public PersonDTO(string username, string city, int age, string phoneNumber)
        {
            Username = username;
            City = city;
            Age = age;
            PhoneNumber = phoneNumber;
        }

        public string Username { get; set; }
        public string City { get; set; }
        public int Age { get; set; }
        public string PhoneNumber { get; set; }
    }
}
