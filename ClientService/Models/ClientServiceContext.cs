using Microsoft.EntityFrameworkCore;

namespace ClientService.Models
{
    public class ClientServiceContext : DbContext
    {
        public ClientServiceContext(DbContextOptions<ClientServiceContext> options) : base(options) { }
    }
}
