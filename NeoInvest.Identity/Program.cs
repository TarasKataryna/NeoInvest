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
	options.UseNpgsql(builder.Configuration.GetConnectionString("userDb"));
});

builder.Services.AddProblemDetails();

builder.Services.AddJwtAuthentication(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.UseExceptionHandler();

app.MapRegisterUser();
app.MapLoginUser();

app.Run();
