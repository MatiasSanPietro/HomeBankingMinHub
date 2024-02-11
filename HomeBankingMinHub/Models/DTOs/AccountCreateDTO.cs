﻿namespace HomeBankingMinHub.Models.DTOs
{
    public class AccountCreateDTO
    {
        public long Id { get; set; }
        public string Number { get; set; }
        public DateTime CreationDate { get; set; }
        public double Balance { get; set; }
    }
}