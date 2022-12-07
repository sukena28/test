using ConsoleApp1;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutoMapper;

var builder = Host.CreateDefaultBuilder()
               .ConfigureServices((hostContext, services) =>
               {
                   services.AddAutoMapper(typeof(Program).Assembly);
                   services.AddTransient<ChainService>();


               }).UseConsoleLifetime();


var app = builder.Build();

var service = app.Services.GetService<ChainService>();







var result =   service!.GetChainRequest();

Console.WriteLine(result);

