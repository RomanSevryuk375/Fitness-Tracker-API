namespace FitnessTracker.Business.Abstractions;

public interface IAuthService
{
    Task<string> LoginUserAsync(string login, string password, CancellationToken ct);
    Task RegisterUserAsync(string login, string password, CancellationToken ct);
}