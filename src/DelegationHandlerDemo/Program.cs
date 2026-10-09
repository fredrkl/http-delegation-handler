using DelegationHandlerDemo.DemoClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHttpClient<IDemoClient>(client => client.BaseAddress = new Uri("https://api.example.com/"));

builder.Build();

Console.WriteLine("Hello, World!");
