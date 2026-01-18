var builder = DistributedApplication.CreateBuilder(args);

var server = builder.AddPostgres("postgres-server");
var walletDb = server.AddDatabase("walletdb");

var dbMigrator = builder.AddProject<Projects.NeoInvest_DbMigrator>("dbmigrator").WithReference(walletDb);
var walletService = builder.AddProject<Projects.NeoInvest_WalletService>("wallet").WithReference(walletDb).WaitForCompletion(dbMigrator);

builder.Build().Run();
