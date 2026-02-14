using FluentValidation;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using NeoInvest.Identity.Configuration;
using NeoInvest.Identity.Data;
using Microsoft.EntityFrameworkCore;
using NeoInvest.Identity.Feature.Users.LoginUser;
using NeoInvest.Identity.Feature.Users.RegisterUser;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using NeoInvest.Identity.Infrastructure;
using MassTransit;
using NeoInvest.Identity.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddMediatR(c =>
{
	c.RegisterServicesFromAssembly(typeof(Program).Assembly);
	c.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddDbContext<IdentityContext>((sp, options) =>
{
	options.UseNpgsql(builder.Configuration.GetConnectionString("userdb"));
});

builder.Services.AddIdentity<User, Role>().AddEntityFrameworkStores<IdentityContext>();

builder.Services.AddMassTransit(configure =>
{
	configure.AddEntityFrameworkOutbox<IdentityContext>(options =>
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

builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

builder.Services.AddProblemDetails();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.MapRegisterUser();
app.MapLoginUser();

app.Run();
