using ChaoticCupid.Server.Models;
using ChaoticCupid.Shared.Models;

namespace ChaoticCupid.Server.Services
{
    public interface IMatchmakingService
    {
        void RegisterPerson(PersonDTO personDTO, string connectionId);
        void ConfirmLetterReceived(string connectionId);
        void BlockUser(string connectionId, string username);
        IEnumerable<Person> getAllPersons();
        Person? FindBestMatch(Person sender);
    }
}