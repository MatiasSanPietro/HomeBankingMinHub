using ClientService.RemoteModel;
using ClientService.RemoteService.Interfaces;
using System.Text;
using System.Text.Json;

namespace ClientService.RemoteService
{
    public class TransactionService : ITransactionService
    {
        private readonly IHttpClientFactory _httpClient;

        public TransactionService(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<(bool resultado, TransactionRemote transaction, string errorMessage)> GetTransaction(long clientId)
        {
            try
            {
                var client = _httpClient.CreateClient("Transaction");
                var response = await client.GetAsync($"api/Transaction/{clientId}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true,
                    };
                    var result = JsonSerializer.Deserialize<TransactionRemote>(content, options);
                    return (true, result, null);
                }
                return (false, null, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }
        public async Task<(bool resultado, string errorMessage)> PostTransaction(TransactionRemote transactionRemote)
        {
            try
            {
                var client = _httpClient.CreateClient("Transaction");
                var json = JsonSerializer.Serialize(transactionRemote);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("api/transaction", content);

                if (response.IsSuccessStatusCode)
                {
                    return (true, null);
                }
                return (false, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
