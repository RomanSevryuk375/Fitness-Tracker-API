namespace FitnessTracker.Business.Secure;

public class JwtOptions
{
    public string SecretKey { get; set; } = string.Empty;
    public int ExpiresHours { get; set; }
}
