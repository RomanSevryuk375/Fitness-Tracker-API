using FitnessTracker.Business.Abstractions;
using FitnessTracker.Core.Abstractions;
using FitnessTracker.Core.Entities;

namespace FitnessTracker.Business.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _repository;
    private readonly IJwtProvider _jwtProvider;
    private readonly IMyPasswordHasher _myPasswordHasher;

    public AuthService(IUserRepository repository, IJwtProvider jwtProvider, IMyPasswordHasher myPasswordHasher)
    {
        _repository = repository;
        _jwtProvider = jwtProvider;
        _myPasswordHasher = myPasswordHasher;
    }

    public async Task<string> LoginUserAsync(string login, string password, CancellationToken ct)
    {
        var user = await _repository.GetByLoginAsync(login, ct)
                ?? throw new ArgumentException("User not found");

        if (!_myPasswordHasher.Verify(password, user.PasswordHash))
            throw new ArgumentException("Invalid password");

        return _jwtProvider.GenerateToken(user);
    }

    public async Task RegisterUserAsync(string login, string password, CancellationToken ct)
    {
        if (await _repository.ExistsAsync(login, ct))
            throw new ArgumentException("User already exists");

        var hashedPassword = _myPasswordHasher.Generate(password);

        var (user, errors) = UserEntity.Create(
            Guid.NewGuid().ToString(),
            login,
            hashedPassword,
            DateTime.UtcNow);

        if (errors.Any())
            throw new ArgumentException(string.Join(", ", errors));

        await _repository.AddAsync(user!, ct);
    }
}
