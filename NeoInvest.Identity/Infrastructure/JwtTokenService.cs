using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NeoInvest.Identity.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NeoInvest.Identity.Infrastructure;

public class JwtTokenService(IOptions<JwtOptions> jwtSettings)
{
	private readonly JwtOptions _jwtSettings = jwtSettings.Value ?? throw new ArgumentNullException(nameof(jwtSettings));

	public string GenerateToken(User user, ICollection<string>? roles)
	{
		var claims = new List<Claim>
		{
			new(ClaimTypes.NameIdentifier, user.Id.ToString()),
			new(ClaimTypes.Name, user.Email!),
		};

		claims.AddRange(roles?.Select(role => new Claim(ClaimTypes.Role, role!)) ?? []);

		var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
		var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

		var token = new JwtSecurityToken(
			issuer: _jwtSettings.Issuer,
			audience: _jwtSettings.Audience,
			claims: claims,
			expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpireMinutes),
			signingCredentials: signingCredentials
		);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}
}
