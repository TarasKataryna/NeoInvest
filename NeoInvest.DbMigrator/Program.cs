using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NeoInvest.DbMigrator;
using NeoInvest.WalletService.Data;

var builder = Host.CreateApplicationBuilder(args);

builder.AddNpgsqlDbContext<WalletDbContext>("walletdb");
builder.AddNpgsqlDbContext<IdentityDbContext>("userdb");

builder.AddServiceDefaults();

var host = builder.Build();

using var scope = host.Services.CreateScope();

var contexts = new DbContext[] { 
	scope.ServiceProvider.GetRequiredService<WalletDbContext>(), 
	scope.ServiceProvider.GetRequiredService<IdentityDbContext>() 
};

foreach (var context in contexts)
{
	await Migrate.MigrateWithRetry(context);
}