using FitnessTracker.Business.Abstractions;
using FitnessTracker.Core.AggregateRoots.Users;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FitnessTracker.Business.Secure;

public class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _options;

    public JwtProvider(IOptions<JwtOptions> options) => _options = options.Value;
    public string GenerateToken(User user)
    {
        Claim[] claims = [new(ClaimTypes.NameIdentifier, user.Id)];

        var singinCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            signingCredentials: singinCredentials,
            expires: DateTime.UtcNow.AddHours(_options.ExpiresHours)
            );

        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenValue.ToString();
    }
}
