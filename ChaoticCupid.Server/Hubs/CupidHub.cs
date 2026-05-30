using ChaoticCupid.Server.Services;
using ChaoticCupid.Shared.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaoticCupid.Server.Hubs
{
    // this hub will handle all the real-time communication between the server and the clients
    public class CupidHub : Hub
    {
        private readonly IMatchmakingService _matchmakingService;
        public CupidHub(IMatchmakingService  matchmakingService)
        {
            _matchmakingService = matchmakingService;
        }
       
        // method called by the client when logging in
        public async Task InitSinglePerson(PersonDTO personDTO){
            _matchmakingService.RegisterPerson(personDTO, Context.ConnectionId);
        }

        // method called by the client when receiving a new letter
        public async Task ConfirmLetterReceived() {
            _matchmakingService.ConfirmLetterReceived(Context.ConnectionId);
        }

        // method called by the client when blocking a user
        public async Task BlockUser(string username) {
            _matchmakingService.BlockUser(Context.ConnectionId, username);
        }
    }
}