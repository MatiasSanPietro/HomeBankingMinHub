using HomeBankingMinHub.Models;
using HomeBankingMinHub.Models.DTOs;
using HomeBankingMinHub.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HomeBankingMinHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private IClientRepository _clientRepository;
        private IAccountRepository _accountRepository;
        private ILoanRepository _loanRepository;
        private IClientLoanRepository _clientLoanRepository;
        private ITransactionRepository _transactionRepository;

        public LoansController(IClientRepository clientRepository, IAccountRepository accountRepository, ILoanRepository loanRepository, IClientLoanRepository clientLoanRepository, ITransactionRepository transactionRepository)
        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
            _loanRepository = loanRepository;
            _clientLoanRepository = clientLoanRepository;
            _transactionRepository = transactionRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var loans = _loanRepository.GetAllLoans();
                var loansDTO = new List<LoanDTO>();

                foreach (Loan loan in loans)
                {
                    var newLoanDTO = new LoanDTO
                    {
                        Id = loan.Id,
                        Name = loan.Name,
                        MaxAmount = loan.MaxAmount,
                        Payments = loan.Payments,
                    };
                    loansDTO.Add(newLoanDTO);
                }
                return Ok(loansDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }
        [HttpPost]
        public IActionResult Post([FromBody] LoanApplicationDTO loanApplicationDTO)
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                
                if (string.IsNullOrEmpty(email))
                {
                    return StatusCode(403, "No hay clientes logeados");
                }

                if (loanApplicationDTO.Amount <= 0)
                {
                    return StatusCode(403, "El monto del prestamo no puede ser 0 o menos");
                }

                if (String.IsNullOrEmpty(loanApplicationDTO.ToAccountNumber))
                {
                    return StatusCode(403, "Cuenta de destino no proporcionada");
                }

                if (String.IsNullOrEmpty(loanApplicationDTO.Payments))
                {
                    return StatusCode(403, "Debe elegir la cantidad de pagos");
                }

                long[] allowedLoanIds = [1, 2, 3];

                if (!allowedLoanIds.Contains(loanApplicationDTO.LoanId))
                {
                    return StatusCode(403, "Tipo de prestamo no valido");
                }

                Client client = _clientRepository.FindByEmail(email);

                if (client == null)
                {
                    return StatusCode(403, "El cliente no existe");
                }

                var loan = _loanRepository.FindById(loanApplicationDTO.LoanId);

                if (loan == null)
                {
                    return StatusCode(403, "El prestamo no existe");
                }

                if (!loan.Payments.Split(',').Contains(loanApplicationDTO.Payments))
                {
                    return StatusCode(403, "Cantidad de cuotas no validas");
                }

                if (loanApplicationDTO.Amount > loan.MaxAmount)
                {
                    return StatusCode(403, "El monto no puede sobrepasar el maximo autorizado");
                }

                var account = _accountRepository.FindByNumber(loanApplicationDTO.ToAccountNumber);

                if (account == null)
                {
                    return StatusCode(403, "La cuenta de destino no existe");
                }

                if (account.ClientId != client.Id)
                {
                    return StatusCode(403, "La cuenta no pertenece al cliente actual");
                }

                Account updatedAccount = account;
                updatedAccount.Balance = account.Balance + loanApplicationDTO.Amount;

                ClientLoan newClientLoan = new ClientLoan
                {
                    ClientId = client.Id,
                    LoanId = loanApplicationDTO.LoanId,
                    Amount = loanApplicationDTO.Amount + (loanApplicationDTO.Amount * 0.2),
                    Payments = loanApplicationDTO.Payments,
                };
                _clientLoanRepository.Save(newClientLoan);

                Transaction newTransaction = new Transaction
                {
                    Amount = loanApplicationDTO.Amount,
                    Description = $"{loan.Name} loan approved",
                    Type = TransactionType.CREDIT,
                    AccountId = account.Id,
                    Date = DateTime.Now,
                };
                _transactionRepository.Save(newTransaction);
                _accountRepository.Save(account);

                return Ok(newClientLoan);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }
    }
}
