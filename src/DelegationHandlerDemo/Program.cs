using DelegationHandlerDemo.DemoClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

var hostBuilder = Host.CreateApplicationBuilder();

hostBuilder.Services.AddOptions<LinkMobilityOptions>()
    .Bind(hostBuilder.Configuration.GetSection("LinkMobility"));

hostBuilder.Services.AddHttpClient(TokenProvider.HttpClientName);
hostBuilder.Services.AddSingleton<ITokenProvider, TokenProvider>();
hostBuilder.Services.AddTransient<CustomDelegatingHandler>();

hostBuilder.Services.AddHttpClient<IDemoClient, DemoClient>(client => client.BaseAddress = new Uri("https://api.example.com/")).AddHttpMessageHandler<CustomDelegatingHandler>();

Console.WriteLine(hostBuilder.Configuration["LinkMobility:ClientId"]);

var host = hostBuilder.Build();
//LinkMobilityOptions options = host.Services.GetRequiredService<IOptions<LinkMobilityOptions>>().Value;
//Console.WriteLine($"LinkMobilityOptions: ClientId={options.ClientId}");

//var httpClient = host.Services.GetRequiredService<IDemoClient>();
//var restult = await httpClient.GetDataAsync("https://httpbin.org/");
//Console.WriteLine(restult);
