using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WalletService.Data;

var builder = Host.CreateApplicationBuilder(args);

builder.AddNpgsqlDbContext<WalletDbContext>("walletdb");
builder.AddServiceDefaults();

var host = builder.Build();

using var scope = host.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<WalletDbContext>();

Console.WriteLine("Applying migrations...");
await dbContext.Database.MigrateAsync();
Console.WriteLine("Migrations applied.");
