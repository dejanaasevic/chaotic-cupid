using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaoticCupid.Shared.Models
{
    public class LetterDTO
    {
        public LetterDTO(string fromUsername, string fromCity, int fromAge, string fromPhone, string message, bool showPhone)
        {
            FromUsername = fromUsername;
            FromCity = fromCity;
            FromAge = fromAge;
            FromPhone = fromPhone;
            Message = message;
            ShowPhone = showPhone;
        }

        public string FromUsername { get; set; }
        public string FromCity { get; set; }
        public int FromAge { get; set; }
        public string FromPhone { get; set; }
        public string Message { get; set; }
        public bool ShowPhone { get; set; }
    }
}