using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace NeoInvest.DbMigrator;

internal class Migrate
{
	public static async Task MigrateWithRetry(DbContext dbContext)
	{
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
			catch (NpgsqlException)
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
	}
}
