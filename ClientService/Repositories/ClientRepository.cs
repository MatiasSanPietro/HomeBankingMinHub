using ClientService.Models;
using ClientService.Repositories.Interfaces;

namespace ClientService.Repositories
{
    public class ClientRepository : RepositoryBase<Client>, IClientRepository
    {
        public ClientRepository(ClientServiceContext repositoryContext) : base(repositoryContext)
        {
        }

        public Client FindById(long id)
        {
            return FindByCondition(client => client.Id == id)
                //.Include(client => client.Accounts)
                //.Include(client => client.ClientLoans)
                //.ThenInclude(cl => cl.Loan)
                //.Include(client => client.Cards)
                .FirstOrDefault();
        }

        public IEnumerable<Client> GetAllClients()
        {
            return FindAll()
                //.Include(client => client.Accounts)
                //.Include(client => client.ClientLoans)
                //.ThenInclude(cl => cl.Loan)
                //.Include(client => client.Cards)
                .ToList();
        }
        public Client FindByEmail(string email)
        {
            return FindByCondition(client => client.Email.ToUpper() == email.ToUpper())
                //.Include(client => client.Accounts)
                //.Include(client => client.ClientLoans)
                //.ThenInclude(cl => cl.Loan)
                //.Include(client => client.Cards)
                .FirstOrDefault();
        }

        public void Save(Client client)
        {
            Create(client);
            SaveChanges();
        }
    }
}
