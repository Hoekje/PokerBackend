using PokerGame.Models;
using PokerGame.DTO;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<GameManager>();
builder.Services.AddOpenApi();


builder.Services.AddOpenApi(); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/", () => "Poker API is running!");

app.MapPost("/games/create", (GameManager manager) =>
{
    var gameId = manager.CreateNewGame();
    return Results.Created($"/games/{gameId}", new { gameId = gameId });
})
.WithName("CreateGame");

app.MapGet("/games/{gameId}/status", (string gameId, GameManager manager) =>
{
    var table = manager.GetGame(gameId);
    if (table == null) return Results.NotFound("Game not found");
    return Results.Ok(new GameStatusDto(table.players.Count, table.pot, table.communityCards, table.currentPhase.ToString()));
}).WithName("GetGameStatus");


app.Run();