namespace HomeBankingMinHub.Models
{
    public class DbInitializer
    {
    public static void Initialize(HomeBankingContext context)
    {
        if (!context.Clients.Any())
        {
            var clients = new Client[]
            {
                    new Client { Email = "vcoronado@gmail.com", FirstName="Victor", LastName="Coronado", Password="123456"},
                    new Client { Email = "vcoronado1@gmail.com", FirstName="Victor1", LastName="Coronado1", Password="1234561"},
                    new Client { Email = "vcoronado2@gmail.com", FirstName="Victor2", LastName="Coronado2", Password="1234562"}
            };

            context.Clients.AddRange(clients);
            context.SaveChanges();
        }
            if (!context.Account.Any())
            {
                var accountVictor = context.Clients.FirstOrDefault(c => c.Email == "vcoronado@gmail.com");
                var accountVictor1 = context.Clients.FirstOrDefault(c => c.Email == "vcoronado1@gmail.com");
                var accountVictor2 = context.Clients.FirstOrDefault(c => c.Email == "vcoronado2@gmail.com");
                if (accountVictor != null)
                {
                    var accounts = new Account[]
                    {
                        new Account {ClientId = accountVictor.Id, CreationDate = DateTime.Now, Number = "VIN001", Balance = 1000 },
                        new Account {ClientId = accountVictor1.Id, CreationDate = DateTime.Now, Number = "VIN002", Balance = 0 },
                        new Account {ClientId = accountVictor2.Id, CreationDate = DateTime.Now, Number = "VIN003", Balance = 500 }
                    };
                    foreach (Account account in accounts)
                    {
                        context.Account.Add(account);
                    }
                    context.SaveChanges();
                }
            }
            if (!context.Transactions.Any())
            {

                var account1 = context.Account.FirstOrDefault(c => c.Number == "VIN001");

                if (account1 != null)
                {
                    var transactions = new Transaction[]

                    {
                        new Transaction { AccountId= account1.Id, Amount = 10000, Date= DateTime.Now.AddHours(-5), Description = "Transferencia reccibida", Type = TransactionType.CREDIT.ToString() },
                        new Transaction { AccountId= account1.Id, Amount = -2000, Date= DateTime.Now.AddHours(-6), Description = "Compra en tienda mercado libre", Type = TransactionType.DEBIT.ToString() },
                        new Transaction { AccountId= account1.Id, Amount = -3000, Date= DateTime.Now.AddHours(-7), Description = "Compra en tienda xxxx", Type = TransactionType.DEBIT.ToString() },
                    };

                    foreach (Transaction transaction in transactions)

                    {
                        context.Transactions.Add(transaction);
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}
