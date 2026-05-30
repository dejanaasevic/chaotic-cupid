using ChaoticCupid.Server.Models;
using ChaoticCupid.Shared.Models;
using System.Security.Cryptography;

namespace ChaoticCupid.Server.Services
{
    public class MatchmakingService : IMatchmakingService
    {
        // lock object for thread safety
        private readonly object _lock = new object();

        // list for storing registered persons
        private readonly List<Person> _persons = new List<Person>();

        // method to register a person
        public void RegisterPerson(PersonDTO personDTO, string connectionId)
        {
            Person person = new Person(personDTO.Username, personDTO.City, personDTO.Age, personDTO.PhoneNumber);
            person.ConnectionId = connectionId;

            lock (_lock)
            {
                // check if the person is already registered, if not add them to the list
                if (!_persons.Any(p => p.Username == person.Username))
                {
                    _persons.Add(person);
                }
            }
        }

        // method to get all registered persons
        public IEnumerable<Person> getAllPersons()
        {
            return _persons;
        }

        // method to confirm that a letter has been received
        public void ConfirmLetterReceived(string connectionId)
        {
            Person? person = _persons.FirstOrDefault(p => p.ConnectionId == connectionId);
            if (person == null)
            {
                throw new Exception("Person not found");
            }

            // set the waiting confirmation flag to false
            person.IsWaitingConfirmation = false;
        }

        // method to block a person from sending letters
        public void BlockUser(string connectionId, string username)
        {
            Person? person = _persons.FirstOrDefault(p => p.ConnectionId == connectionId);
            if (person == null)
            {
                throw new Exception("Person not found");
            }

            // add the username to the blocked users list if it's not already there
            if (!person.BlockedUsers.Contains(username))
            {
                person.BlockedUsers.Add(username);
            }
        }

        // method to find the best match for a person
        public Person? FindBestMatch(Person sender)
        {
            int bestScore = int.MinValue;
            Person match = null;

            foreach (Person person in _persons)
            {
                // skip if the person is the sender
                if (person.Username == sender.Username)
                {
                    continue;
                }

                // skip if confirmation if the previous letter hasn't been received
                if (person.IsWaitingConfirmation)
                {
                    continue;
                }

                // skip if the person is blocked by the sender or has blocked the sender
                if (sender.BlockedUsers.Contains(person.Username) || person.BlockedUsers.Contains(sender.Username))
                {
                    continue;
                }

                // calculate the score 
                int score = 0;
                if (person.City == sender.City)
                {
                    score += 30;
                }

                if (Math.Abs(person.Age - sender.Age) <= 2)
                {
                    score += 20;
                }
                
                score += calculateRandomPoints();

                // update the best match if the score is higher that the current best
                if(score > bestScore)
                {
                    bestScore = score;
                    match = person;
                }
            }
            return match;
        }

        // method to calculate random points using RNGCryptoServiceProvider
        private int calculateRandomPoints()
        {
            var rng = new RNGCryptoServiceProvider();
            byte[] bytes = new byte[4];
            rng.GetBytes(bytes);
            int randomPoints = Math.Abs(BitConverter.ToInt32(bytes, 0)) % 101;
            return randomPoints;
        }
    }
}