using ChaoticCupid.Server.Hubs;

var builder = WebApplication.CreateBuilder(args);

// register signalR services for real-time communication
builder.Services.AddSignalR();

var app = builder.Build();

app.UseHttpsRedirection();

// endpoit for the CupidHub, allowing clients to connect and communicate in real-time
app.MapHub<CupidHub>("/cupid");

app.Run();
