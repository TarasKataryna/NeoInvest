using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace NeoInvest.Identity.Infrastructure;

public static class AuthExtension
{
	public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddOptions<JwtOptions>().Bind(configuration.GetSection(nameof(JwtOptions)));
		var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();

		services.AddAuthentication(opt =>
		{
			opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		})
		.AddJwtBearer(options =>
		{
			options.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateLifetime = true,
				ValidateIssuerSigningKey = true,
				ValidIssuer = jwtOptions!.Issuer,
				ValidAudience = jwtOptions.Audience,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
				ClockSkew = TimeSpan.Zero
			};
		});
	}
}
