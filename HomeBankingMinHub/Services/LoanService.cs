using HomeBankingMinHub.Models;
using HomeBankingMinHub.Models.DTOs;
using HomeBankingMinHub.Repositories.Interfaces;
using HomeBankingMinHub.Services.Interfaces;

namespace HomeBankingMinHub.Services
{
    public class LoanService : ILoanService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ILoanRepository _loanRepository;
        private readonly IClientLoanRepository _clientLoanRepository;
        private readonly ITransactionRepository _transactionRepository;

        public LoanService(IClientRepository clientRepository, IAccountRepository accountRepository, ILoanRepository loanRepository, IClientLoanRepository clientLoanRepository, ITransactionRepository transactionRepository)
        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
            _loanRepository = loanRepository;
            _clientLoanRepository = clientLoanRepository;
            _transactionRepository = transactionRepository;
        }
        public class LoanServiceException : Exception
        {
            public LoanServiceException(string message) : base(message) { }
        }
        List<LoanDTO> ILoanService.GetAllLoans()
        {
            var loans = _loanRepository.GetAllLoans();
            var loansDTO = new List<LoanDTO>();

            foreach (var loan in loans)
            {
                LoanDTO loanDTO = new LoanDTO(loan);
                loansDTO.Add(loanDTO);
            }

            return loansDTO;
        }
        ClientLoan ILoanService.ApplyForLoan(string email, LoanApplicationDTO loanApplicationDTO)
        {
            if (loanApplicationDTO.Amount <= 0)
            {
                throw new LoanServiceException("El monto del prestamo no puede ser 0 o menos");
            }

            if (String.IsNullOrEmpty(loanApplicationDTO.ToAccountNumber))
            {
                throw new LoanServiceException("Cuenta de destino no proporcionada");
            }

            if (String.IsNullOrEmpty(loanApplicationDTO.Payments))
            {
                throw new LoanServiceException("Debe elegir la cantidad de pagos");
            }

            long[] allowedLoanIds = [1, 2, 3];

            if (!allowedLoanIds.Contains(loanApplicationDTO.LoanId))
            {
                throw new LoanServiceException("Tipo de prestamo no valido");
            }

            var client = _clientRepository.FindByEmail(email);

            if (client == null)
            {
                throw new LoanServiceException("El cliente no existe");
            }

            var loan = _loanRepository.FindById(loanApplicationDTO.LoanId);

            if (loan == null)
            {
                throw new LoanServiceException("El prestamo no existe");
            }

            if (!loan.Payments.Split(',').Contains(loanApplicationDTO.Payments))
            {
                throw new LoanServiceException("Cantidad de cuotas no validas");
            }

            if (loanApplicationDTO.Amount > loan.MaxAmount)
            {
                throw new LoanServiceException("El monto no puede sobrepasar el maximo autorizado");
            }

            var account = _accountRepository.FindByNumber(loanApplicationDTO.ToAccountNumber);

            if (account == null)
            {
                throw new LoanServiceException("La cuenta de destino no existe");
            }

            if (account.ClientId != client.Id)
            {
                throw new LoanServiceException("La cuenta no pertenece al cliente actual");
            }

            ClientLoan newClientLoan = new()
            {
                ClientId = client.Id,
                LoanId = loanApplicationDTO.LoanId,
                Amount = loanApplicationDTO.Amount + (loanApplicationDTO.Amount * 0.2),
                Payments = loanApplicationDTO.Payments,
            };
            _clientLoanRepository.Save(newClientLoan);

            Transaction newTransaction = new()
            {
                Amount = loanApplicationDTO.Amount,
                Description = $"{loan.Name} loan approved",
                Type = TransactionType.CREDIT,
                AccountId = account.Id,
                Date = DateTime.Now,
            };
            _transactionRepository.Save(newTransaction);

            Account updatedAccount = account;
            updatedAccount.Balance = account.Balance + loanApplicationDTO.Amount;
            _accountRepository.Save(account);

            return newClientLoan;
        }
    }
}
