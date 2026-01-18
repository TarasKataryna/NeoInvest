using FluentValidation;
using NeoInvest.WalletService.Features.Wallets.DepositFunds;
using NeoInvest.WalletService.Features.Wallets.WithdrawFunds;
using WalletService.Configuration;
using WalletService.Data;
using WalletService.Features.Wallets.CreateWallet;
using WalletService.Features.Wallets.GetWallet;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.AddNpgsqlDbContext<WalletDbContext>("walletdb");

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Services.AddMediatR(c =>
{
    c.RegisterServicesFromAssembly(typeof(Program).Assembly);
    c.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapCreateWallet();
app.MapGetWallet();
app.MapWithdrawFunds();
app.MapDepositFunds();

app.Run();
