using ChaoticCupid.Server.Hubs;
using ChaoticCupid.Server.Services;
using ChaoticCupid.Server.Workers;

var builder = WebApplication.CreateBuilder(args);

// register signalR services for real-time communication
builder.Services.AddSignalR();

// register the matchmaking service as a singleton, so it will be shared across the entire application
builder.Services.AddSingleton<IMatchmakingService, MatchmakingService>();

// register the worker service that will run in the background and send letters to the clients every minute
builder.Services.AddHostedService<CupidWorker>();

var app = builder.Build();

app.UseHttpsRedirection();

// endpoit for the CupidHub, allowing clients to connect and communicate in real-time
app.MapHub<CupidHub>("/cupid");

app.Run();
