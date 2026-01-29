using FitnessTracker.Core.Abstractions;
using System.Xml.Linq;

namespace FitnessTracker.Core.Entities;

public class UserEntity : IDocument
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Login { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<WorkoutEntity> Workouts { get; private set; } = [];

    private UserEntity() { }

    private UserEntity(string id, string login, string passwordHash, DateTime createdAt)
    {
        Id = id;
        Login = login;
        PasswordHash = passwordHash;
        CreatedAt = createdAt;
    }

    public static (UserEntity? user, List<string> errors) Create(string id, string login, string passwordHash, DateTime createdAt)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(id))
            errors.Add("Id can not be empty");
        if (string.IsNullOrWhiteSpace(login))
            errors.Add("Login can not be empty");
        if (string.IsNullOrWhiteSpace(passwordHash))
            errors.Add("PasswordHash can not be empty");

        if (errors.Any())
            return (null, errors);

        var exercise = new UserEntity(id, login, passwordHash, createdAt);

        return (exercise, []);
    }
}
