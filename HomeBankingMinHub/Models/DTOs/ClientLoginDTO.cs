﻿namespace HomeBankingMinHub.Models.DTOs
{
    public class ClientLoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
    }
}