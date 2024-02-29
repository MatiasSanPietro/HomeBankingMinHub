namespace ClientService.RemoteModel
{
    public class TransactionRemote
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public long ClientId { get; set; }
    }
}
