using ChaoticCupid.Shared.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaoticCupid.Server.Hubs
{
    // This hub will handle all the real-time communication between the server and the clients
    public class CupidHub : Hub
    {
       
        // called by the client when logging in
        public async Task InitSinglePerson(PersonDTO personDTO){}

        // called by the client when receiving a new letter
        public async Task ConfirmLetterReceived() { }

        // called by the client when blocking a user
        public async Task BlockUser(string username) { }

    }
}