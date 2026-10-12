using DelegationHandlerDemo.DemoClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var hostBuilder = Host.CreateApplicationBuilder();
hostBuilder.Services.AddTransient<ITokenProvider, TokenProvider>();
hostBuilder.Services.AddTransient<CustomDelegatingHandler>();
hostBuilder.Services.AddHttpClient<IDemoClient, DemoClient>(client => client.BaseAddress = new Uri("https://api.example.com/")).AddHttpMessageHandler<CustomDelegatingHandler>();

var host = hostBuilder.Build();
var httpClient = host.Services.GetRequiredService<IDemoClient>();
var restult = await httpClient.GetDataAsync("https://httpbin.org/");
Console.WriteLine(restult);
