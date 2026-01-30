using FitnessTracker.Business.Abstractions;

namespace FitnessTracker.Business.Secure;

public class MyPasswordHasher : IMyPasswordHasher
{
    public string Generate(string password) =>
        BCrypt.Net.BCrypt.EnhancedHashPassword(password);

    public bool Verify(string password, string passwordHash) =>
        BCrypt.Net.BCrypt.EnhancedVerify(password, passwordHash);
}
