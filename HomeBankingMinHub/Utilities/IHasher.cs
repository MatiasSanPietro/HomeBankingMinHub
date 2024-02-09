namespace HomeBankingMinHub.Utilities
{
    public interface IHasher
    {
        string HashPassword(string password, out string salt);
        bool VerifyPassword(string password, string hashedPassword, string salt);
    }
}
