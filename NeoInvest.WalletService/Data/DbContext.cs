using Microsoft.EntityFrameworkCore;
using NeoInvest.WalletService.Domain.Enitites;

namespace NeoInvest.WalletService.Data;

public class WalletDbContext(DbContextOptions<WalletDbContext> options) : DbContext(options)
{
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(WalletDbContext).Assembly);
	}

	public DbSet<Wallet> Wallets { get; set; }
	public DbSet<Transaction> Transactions { get; set; }
}
