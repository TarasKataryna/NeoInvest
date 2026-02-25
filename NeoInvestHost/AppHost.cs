var builder = DistributedApplication.CreateBuilder(args);

var server = builder.AddPostgres("postgres-server");
var walletDb = server.AddDatabase("walletdb");
var userDb = server.AddDatabase("userdb");
var sagaDb = server.AddDatabase("sagadb");

var messaging = builder.AddRabbitMQ(
		"messaging",
		builder.AddParameter("rabbit-user", secret: true),
		builder.AddParameter("rabbit-password", secret: true))
	.WithManagementPlugin();

var dbMigrator = builder.AddProject<Projects.NeoInvest_DbMigrator>("dbmigrator")
	.WithReference(walletDb)
	.WithReference(userDb)
	.WithReference(sagaDb)
	.WaitFor(server);

var identityService = builder.AddProject<Projects.NeoInvest_Identity>("identity")
	.WithReference(userDb)
	.WithReference(messaging)
	.WaitForCompletion(dbMigrator);

var walletService = builder.AddProject<Projects.NeoInvest_WalletService>("wallet")
	.WithReference(walletDb)
	.WithReference(messaging)
	.WaitForCompletion(dbMigrator);

var sagaService = builder.AddProject<Projects.NeoInvest_WalletService>("saga")
	.WithReference(sagaDb)
	.WithReference(messaging)
	.WaitForCompletion(dbMigrator);

builder.AddProject<Projects.NeoInvest_Gateway>("gateway")
	.WithReference(walletService)
	.WithReference(identityService)
	.WithExternalHttpEndpoints();

builder.AddProject<Projects.NeoInvest_Saga>("neoinvest-saga");

builder.Build().Run();
