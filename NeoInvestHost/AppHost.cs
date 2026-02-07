var builder = DistributedApplication.CreateBuilder(args);

var server = builder.AddPostgres("postgres-server");
var walletDb = server.AddDatabase("walletDb");
var userDB = server.AddDatabase("userDb");

var dbMigrator = builder.AddProject<Projects.NeoInvest_DbMigrator>("dbmigrator").WithReference(walletDb).WaitFor(walletDb);

var identityService = builder.AddProject<Projects.NeoInvest_Identity>("identity").WithReference(userDB).WaitForCompletion(dbMigrator);

var walletService = builder.AddProject<Projects.NeoInvest_WalletService>("wallet").WithReference(walletDb).WaitForCompletion(dbMigrator);

builder.Build().Run();
