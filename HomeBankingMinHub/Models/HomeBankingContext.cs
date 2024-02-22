﻿using Microsoft.EntityFrameworkCore;

namespace HomeBankingMinHub.Models
{
    public class HomeBankingContext : DbContext
    {
        public HomeBankingContext(DbContextOptions<HomeBankingContext> options) : base(options) { }
        // propiedad DbSet representa las colecciones de entidades en la bd
        public DbSet<Client> Clients { get; set; }
        public DbSet<Account> Account { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<ClientLoan> ClientLoans { get; set; }
        public DbSet<Card> Cards { get; set; }
    }
}
