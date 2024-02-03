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
                        new Account {ClientId = accountVictor.Id, CreationDate = DateTime.Now, Number = string.Empty, Balance = 0 },
                        new Account {ClientId = accountVictor1.Id, CreationDate = DateTime.Now, Number = string.Empty, Balance = 0 },
                        new Account {ClientId = accountVictor2.Id, CreationDate = DateTime.Now, Number = string.Empty, Balance = 0 }
                    };
                    foreach (Account account in accounts)
                    {
                        context.Account.Add(account);
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}
