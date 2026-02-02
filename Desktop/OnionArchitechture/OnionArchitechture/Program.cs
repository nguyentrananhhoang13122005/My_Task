using Microsoft.Extensions.DependencyInjection;
using Library.Core.Interfaces;
using Library.Infrastructure.InMemory;
using Library.Service;
using Library.Presentation;


var services = new ServiceCollection();

services.AddSingleton<IMediaRepository, InMemoryMediaRepository>();
services.AddScoped<IUnitOfWork, InMemoryUnitOfWork>();


services.AddScoped<MediaPlayerService>();


services.AddScoped<MediaMenu>();

var provider = services.BuildServiceProvider();

var menu = provider.GetRequiredService<MediaMenu>();
await menu.RunAsync();