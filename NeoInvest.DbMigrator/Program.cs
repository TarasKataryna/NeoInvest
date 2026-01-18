using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NeoInvest.WalletService.Data;
using Npgsql;

var builder = Host.CreateApplicationBuilder(args);

builder.AddNpgsqlDbContext<WalletDbContext>("walletdb");
builder.AddServiceDefaults();

var host = builder.Build();

using var scope = host.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<WalletDbContext>();

int retryCount = 0;
while (retryCount < 5)
{
	try
	{
		Console.WriteLine("Applying migrations...");
		await dbContext.Database.MigrateAsync();
		Console.WriteLine("Migrations applied.");
		break;
	}
	catch(NpgsqlException)
	{
		retryCount++;
		Console.WriteLine($"Database connection failed. Retrying {retryCount}/5...");
		await Task.Delay(2000);
	}
	catch (Exception ex)
	{
		Console.WriteLine($"An error occurred: {ex.Message}");
		return;
	}
}
