using Yarp.ReverseProxy.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddServiceDiscovery();

var routes = new []
{
	new RouteConfig
	{
		RouteId = "identity-route",
		ClusterId = "identity-cluster",
		Match = new RouteMatch
		{
			Path = "api/auth/{**catch-all}"
		}
	},
	new RouteConfig
	{
		RouteId = "wallet-cluster",
		ClusterId = "wallet-cluster",
		Match = new RouteMatch
		{
			Path = "api/wallet/{**catch-all}"
		}
	}
};
var clusters = new []
{
	new ClusterConfig
	{
		ClusterId = "identity-cluster",
		Destinations = new Dictionary<string, DestinationConfig>
		{
			{ "default", new DestinationConfig { Address = "http://identity" } }
		}
	},
	new ClusterConfig
	{
		ClusterId = "wallet-cluster",
		Destinations = new Dictionary<string, DestinationConfig>
		{
			{ "default", new DestinationConfig { Address = "http://wallet" } }
		}
	}
};

builder.Services.AddReverseProxy()
	.LoadFromMemory(routes, clusters)
	.AddServiceDiscoveryDestinationResolver();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
	app.UseSwagger();
	app.UseSwaggerUI(options =>
	{
		options.SwaggerEndpoint("/api/auth/swagger/v1/swagger.json", "Identity API");
		options.SwaggerEndpoint("/api/wallet/swagger/v1/swagger.json", "Wallet V1");

		options.RoutePrefix = "swagger";
	});
}

app.UseHttpsRedirection();

app.Run();

