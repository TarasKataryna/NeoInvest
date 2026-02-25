using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using NeoInvest.Saga;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddDbContext<NeoInvest.Saga.SagaDbContext>((sp, opt) =>
{
	opt.UseNpgsql(builder.Configuration.GetConnectionString("sagadb"));
});

builder.Services.AddMassTransit(configure =>
{
	configure.AddSagaStateMachine<OnboardingSaga, OnboardingSagaState>()
		.EntityFrameworkRepository(r =>
		{
			r.ConcurrencyMode = ConcurrencyMode.Optimistic;
			r.ExistingDbContext<NeoInvest.Saga.SagaDbContext>();
		});

	configure.AddEntityFrameworkOutbox<NeoInvest.Saga.SagaDbContext>(options =>
	{
		options.UsePostgres();
		options.UseBusOutbox();
	});

	configure.UsingRabbitMq((context, cfg) =>
	{
		cfg.Host(builder.Configuration.GetConnectionString("messaging"));
		cfg.ConfigureEndpoints(context);
	});
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.Run();