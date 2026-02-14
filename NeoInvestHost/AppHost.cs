var builder = DistributedApplication.CreateBuilder(args);

var server = builder.AddPostgres("postgres-server");
var walletDb = server.AddDatabase("walletdb");
var userDB = server.AddDatabase("userdb");

var messaging = builder.AddRabbitMQ(
		"messaging",
		builder.AddParameter("rabbit-user", secret: true),
		builder.AddParameter("rabbit-password", secret: true))
	.WithManagementPlugin();

var dbMigrator = builder.AddProject<Projects.NeoInvest_DbMigrator>("dbmigrator")
	.WithReference(walletDb)
	.WithReference(userDB)
	.WaitFor(server);

var identityService = builder.AddProject<Projects.NeoInvest_Identity>("identity")
	.WithReference(userDB)
	.WithReference(messaging)
	.WaitForCompletion(dbMigrator);

var walletService = builder.AddProject<Projects.NeoInvest_WalletService>("wallet")
	.WithReference(walletDb)
	.WithReference(messaging)
	.WaitForCompletion(dbMigrator);

builder.AddProject<Projects.NeoInvest_Gateway>("gateway")
	.WithReference(walletService)
	.WithReference(identityService)
	.WithExternalHttpEndpoints();

builder.Build().Run();
