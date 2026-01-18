using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NeoInvest.WalletService.Domain.Enitites;

namespace NeoInvest.WalletService.Features.Wallets;

public class WalletConfiguration : IEntityTypeConfiguration<Wallet>
{
	public void Configure(EntityTypeBuilder<Wallet> builder)
	{
		builder.Property(w => w.Currency).HasConversion<string>();

		builder.Property(w => w.Version).IsConcurrencyToken();
	}
}
