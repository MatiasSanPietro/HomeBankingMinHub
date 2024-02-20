using HomeBankingMinHub.Models;
using HomeBankingMinHub.Models.DTOs;
using HomeBankingMinHub.Repositories.Interfaces;
using HomeBankingMinHub.Services.Interfaces;

namespace HomeBankingMinHub.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(IClientRepository clientRepository, IAccountRepository accountRepository, ITransactionRepository transactionRepository)
        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
        }
        public class TransactionServiceException : Exception
        {
            public TransactionServiceException(string message) : base(message) { }
        }

        public void MakeTransfer(string email, TransferDTO transferDTO)
        {
            Client client = _clientRepository.FindByEmail(email);

            if (string.IsNullOrEmpty(transferDTO.FromAccountNumber) ||
                string.IsNullOrEmpty(transferDTO.ToAccountNumber))
            {
                throw new TransactionServiceException("Cuenta de origen o cuenta de destino no proporcionada");
            }

            if (transferDTO.FromAccountNumber == transferDTO.ToAccountNumber)
            {
                throw new TransactionServiceException("No se permite la transferencia a la misma cuenta");
            }

            if (string.IsNullOrEmpty(transferDTO.Description))
            {
                throw new TransactionServiceException("Descripcion no proporcionada");
            }

            if (transferDTO.Amount < 0)
            {
                throw new TransactionServiceException("Monto no valido");
            }

            if (client == null)
            {
                throw new TransactionServiceException("El cliente no existe");
            }

            Account fromAccount = _accountRepository.FindByNumber(transferDTO.FromAccountNumber);

            if (fromAccount == null)
            {
                throw new TransactionServiceException("Cuenta de origen no existe");
            }

            if (fromAccount.ClientId != client.Id)
            {
                throw new TransactionServiceException("La cuenta de origen no pertenece al cliente actual");
            }

            if (fromAccount.Balance < transferDTO.Amount)
            {
                throw new TransactionServiceException("Fondos insuficientes");
            }

            Account toAccount = _accountRepository.FindByNumber(transferDTO.ToAccountNumber);

            if (toAccount == null)
            {
                throw new TransactionServiceException("Cuenta de destino no existe");
            }

            // Insertar transacciones para la transferencia
            _transactionRepository.Save(new Transaction
            {
                Type = TransactionType.DEBIT,
                Amount = transferDTO.Amount * -1,
                Description = $"{transferDTO.Description} {toAccount.Number}",
                AccountId = fromAccount.Id,
                Date = DateTime.Now,
            });

            _transactionRepository.Save(new Transaction
            {
                Type = TransactionType.CREDIT,
                Amount = transferDTO.Amount,
                Description = $"{transferDTO.Description} {fromAccount.Number}",
                AccountId = toAccount.Id,
                Date = DateTime.Now,
            });

            // Actualizar saldos de cuentas
            fromAccount.Balance -= transferDTO.Amount;
            toAccount.Balance += transferDTO.Amount;

            _accountRepository.Save(fromAccount);
            _accountRepository.Save(toAccount);
        }
    }
}
