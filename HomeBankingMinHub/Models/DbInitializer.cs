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
    }
    }
}
