using ChaoticCupid.Server.Hubs;
using ChaoticCupid.Server.Models;
using ChaoticCupid.Server.Services;
using ChaoticCupid.Shared.Models;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics.Metrics;

namespace ChaoticCupid.Server.Workers
{
    public class CupidWorker : BackgroundService
    {
        private readonly IMatchmakingService _matchmakingService;
        private readonly IHubContext<CupidHub> _cupidHubContext;
        public CupidWorker(IMatchmakingService matchmakingService, IHubContext<CupidHub> cupidHubContext)
        {
            _matchmakingService = matchmakingService;
            _cupidHubContext = cupidHubContext;
        }

        // method that will run in the background and send letters to the clients every minute
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                await SendLettersAsync();
            }
        }

        // method that will send letters to the persons based on matchmaking logic
        private async Task SendLettersAsync()
        {
            IEnumerable<Person> persons = _matchmakingService.getAllPersons();
            foreach (Person person in persons)
            {
                Person? match = _matchmakingService.FindBestMatch(person);
                // no match found, skip this person
                if (match == null)
                {
                    continue;
                }

                // generate a random letter from the match to the person
                string[] letters = { "Radujem se našem susretu!", "Zelim da se upoznamo.", "Nisam zainteresovan/a za upoznavanje." };

                Random random = new Random();
                int index = random.Next(letters.Length);

                // only show the phone number if match is interested 
                bool showPhone = index == 2 ? false : true;

                LetterDTO letterDTO = new LetterDTO(match.Username, match.City, match.Age, match.PhoneNumber, letters[index], showPhone);

                // send the letter to the person via signalR
                await _cupidHubContext.Clients.Client(person.ConnectionId).SendAsync("ReceiveLetter", letterDTO);

                // set the waiting confirmation flat to true for reciever
                _matchmakingService.SetWaitingConfirmation(person.ConnectionId);
            }
        }
    }
}