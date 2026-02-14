using FluentValidation;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NeoInvest.WalletService.Data;
using NeoInvest.WalletService.Features.Wallets.CreateWallet;
using NeoInvest.WalletService.Features.Wallets.DepositFunds;
using NeoInvest.WalletService.Features.Wallets.GetWallet;
using NeoInvest.WalletService.Features.Wallets.WithdrawFunds;
using System.Text;
using WalletService.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<PublishDomainEventsInterceptor>();
builder.Services.AddDbContext<WalletDbContext>((sp, options) =>
{
	var interceptor = sp.GetRequiredService<PublishDomainEventsInterceptor>();
	options.AddInterceptors(interceptor);

	options.UseNpgsql(builder.Configuration.GetConnectionString("walletdb"));
});
builder.Services.AddNpgsqlDataSource("walletdb");

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Services.AddMediatR(c =>
{
	c.RegisterServicesFromAssembly(typeof(Program).Assembly);
	c.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddProblemDetails();

builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = builder.Configuration["JwtOptions:Issuer"],
		ValidAudience = builder.Configuration["JwtOptions:Audience"],
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtOptions:Key"]!))
	};
});

builder.Services.AddMassTransit(configure =>
{
	configure.AddConsumers(typeof(Program).Assembly);

	configure.AddEntityFrameworkOutbox<WalletDbContext>(options =>
	{
		options.UsePostgres();
		options.UseBusOutbox();
	});

	configure.UsingRabbitMq((context, cfg) =>
	{
		cfg.Host(builder.Configuration.GetConnectionString("messaging"));
		cfg.ConfigureEndpoints(context);
	});
});

builder.Services.AddLogging(configure => configure.AddConsole());

var app = builder.Build();

app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapCreateWallet();
app.MapGetWallet();
app.MapWithdrawFunds();
app.MapDepositFunds();

app.Run();
