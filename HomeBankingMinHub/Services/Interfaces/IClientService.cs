using HomeBankingMinHub.Models;
using HomeBankingMinHub.Models.DTOs;

namespace HomeBankingMinHub.Services.Interfaces
{
    public interface IClientService
    {
        List<ClientDTO> GetAllClients();
        ClientDTO GetClientById(long id);
        Client CreateClient(ClientRegisterDTO client);
        ClientDTO GetCurrentClient(string email);
    }
}
