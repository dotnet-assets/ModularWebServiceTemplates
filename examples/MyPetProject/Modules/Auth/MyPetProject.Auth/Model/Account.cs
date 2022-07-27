using System.Security.Cryptography;

namespace MyPetProject.Auth.Model;

internal class Account
{
    public Account(string username, UserRole role, string password)
    {
        Username = username;
        Role = role;
        Created = DateTime.UtcNow;
        (PasswordSalt, PasswordHash) = HashPassword(password);
    }

    public int Id { get; private set; }

    public string Username { get; private set; }

    public UserRole Role { get; private set; }

    public DateTime Created { get; private set; }

    public byte[] PasswordHash { get; private set; }

    public byte[] PasswordSalt { get; private set; }

    public bool CheckPassword(string password)
    {
        using var hmac = new HMACSHA512(PasswordSalt);
        byte[] computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(PasswordHash);
    }

    private (byte[] Salt, byte[] Hash) HashPassword(string password)
    {
        using HMACSHA512 hmac = new();
        byte[] hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return (hmac.Key, hash);
    }

    // EF Core requires a default constructor
    private Account()
    {
        Id = default!;
        Username = default!;
        Role = default!;
        Created = default!;
        PasswordHash = default!;
        PasswordSalt = default!;
    }
}