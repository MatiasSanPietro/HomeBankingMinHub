using HomeBankingMinHub.Models;
using HomeBankingMinHub.Models.DTOs;
using HomeBankingMinHub.Repositories.Interfaces;
using HomeBankingMinHub.Services.Interfaces;
using HomeBankingMinHub.Utils;
using HomeBankingMinHub.Utils.Interfaces;

namespace HomeBankingMinHub.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IHasher _hasher;

        public ClientService(IClientRepository clientRepository, IAccountRepository accountRepository, IHasher hasher)
        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
            _hasher = hasher;
        }

        public class ClientServiceException : Exception
        {
            public ClientServiceException(string message) : base(message)
            {
            }
        }

        public List<ClientDTO> GetAllClients()
        {
            var clients = _clientRepository.GetAllClients();
            var clientsDTO = new List<ClientDTO>();

            foreach (Client client in clients)
            {
                var newClientDTO = new ClientDTO
                {
                    Id = client.Id,
                    Email = client.Email,
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                    Accounts = client.Accounts.Select(ac => new AccountDTO
                    {
                        Id = ac.Id,
                        Balance = ac.Balance,
                        CreationDate = ac.CreationDate,
                        Number = ac.Number
                    }).ToList(),
                    Credits = client.ClientLoans.Select(cl => new ClientLoanDTO
                    {
                        Id = cl.Id,
                        LoanId = cl.LoanId,
                        Name = cl.Loan.Name,
                        Amount = cl.Amount,
                        Payments = int.Parse(cl.Payments)
                    }).ToList(),
                    Cards = client.Cards.Select(c => new CardDTO
                    {
                        Id = c.Id,
                        CardHolder = c.CardHolder,
                        Color = c.Color.ToString(),
                        Cvv = c.Cvv,
                        FromDate = c.FromDate,
                        Number = c.Number,
                        ThruDate = c.ThruDate,
                        Type = c.Type.ToString()
                    }).ToList()
                };
                clientsDTO.Add(newClientDTO);
            }
            return clientsDTO;
        }

        public ClientDTO GetClientById(long id)
        {
            var client = _clientRepository.FindById(id);

            if (client == null)
            {
                throw new ClientServiceException("El cliente no existe");
            }

            var clientDTO = new ClientDTO
            {
                Id = client.Id,
                Email = client.Email,
                FirstName = client.FirstName,
                LastName = client.LastName,
                Accounts = client.Accounts.Select(ac => new AccountDTO
                {
                    Id = ac.Id,
                    Balance = ac.Balance,
                    CreationDate = ac.CreationDate,
                    Number = ac.Number
                }).ToList(),
                Credits = client.ClientLoans.Select(cl => new ClientLoanDTO
                {
                    Id = cl.Id,
                    LoanId = cl.LoanId,
                    Name = cl.Loan.Name,
                    Amount = cl.Amount,
                    Payments = int.Parse(cl.Payments)
                }).ToList(),
                Cards = client.Cards.Select(c => new CardDTO
                {
                    Id = c.Id,
                    CardHolder = c.CardHolder,
                    Color = c.Color.ToString(),
                    Cvv = c.Cvv,
                    FromDate = c.FromDate,
                    Number = c.Number,
                    ThruDate = c.ThruDate,
                    Type = c.Type.ToString()
                }).ToList()
            };
            return clientDTO;
        }

        public Client CreateClient(ClientRegisterDTO client)
        {
            // Validaciones para register
            if (string.IsNullOrEmpty(client.Email) ||
                string.IsNullOrEmpty(client.Password) ||
                string.IsNullOrEmpty(client.FirstName) ||
                string.IsNullOrEmpty(client.LastName))
                throw new ClientServiceException("Todos los datos son obligatorios");

            if (!EmailValidations.IsValidEmail(client.Email))
            {
                throw new ClientServiceException("La direccion de correo electronico no es valida");
            }

            if (client.Password.Length < 8)
            {
                throw new ClientServiceException("La contrasenia debe tener por lo menos 8 caracteres");
            }

            // Buscamos si ya existe el usuario
            var user = _clientRepository.FindByEmail(client.Email);

            if (user != null)
            {
                throw new ClientServiceException("Email esta en uso");
            }

            // Hashing de contraseña
            string salt;
            string hashedPassword = _hasher.HashPassword(client.Password, out salt);

            Client newClient = new()
            {
                Email = client.Email,
                Password = hashedPassword,
                Salt = salt,
                FirstName = client.FirstName,
                LastName = client.LastName,
            };

            _clientRepository.Save(newClient);

            // Crear cuenta al registrarse
            var dbUser = _clientRepository.FindByEmail(newClient.Email);

            if (dbUser == null)
            {
                throw new ClientServiceException("Error al crear la cuenta, no hay cliente registrado");
            }

            Account newAccount = new()
            {
                ClientId = dbUser.Id,
                CreationDate = DateTime.Now,
                Balance = 0
            };
            _accountRepository.Save(newAccount);
            return newClient;
        }

        public ClientDTO GetCurrentClient(string email)
        {
            var client = _clientRepository.FindByEmail(email);

            if (client == null)
            {
                throw new ClientServiceException("El cliente no existe");
            }

            var clientDTO = new ClientDTO
            {
                Id = client.Id,
                Email = client.Email,
                FirstName = client.FirstName,
                LastName = client.LastName,
                Accounts = client.Accounts.Select(ac => new AccountDTO
                {
                    Id = ac.Id,
                    Balance = ac.Balance,
                    CreationDate = ac.CreationDate,
                    Number = ac.Number
                }).ToList(),
                Credits = client.ClientLoans.Select(cl => new ClientLoanDTO
                {
                    Id = cl.Id,
                    LoanId = cl.LoanId,
                    Name = cl.Loan.Name,
                    Amount = cl.Amount,
                    Payments = int.Parse(cl.Payments)
                }).ToList(),
                Cards = client.Cards.Select(c => new CardDTO
                {
                    Id = c.Id,
                    CardHolder = c.CardHolder,
                    Color = c.Color.ToString(),
                    Cvv = c.Cvv,
                    FromDate = c.FromDate,
                    Number = c.Number,
                    ThruDate = c.ThruDate,
                    Type = c.Type.ToString()
                }).ToList()
            };
            return clientDTO;
        }
    }
}
