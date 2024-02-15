﻿using HomeBankingMinHub.Models;

namespace HomeBankingMinHub.Repositories.Interfaces
{
    public interface ITransactionRepository
    {
        void Save(Transaction transaction);
        Transaction FindByNumber(long id);
    }
}