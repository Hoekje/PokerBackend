using PokerGame.Models;
using PokerGame.DTO;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<GameManager>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/", () => "Poker API is running!");

// Make this connect to user Database
app.MapPost("/games/create", (GameManager manager) =>
{
    var player = new Player(Guid.NewGuid().ToString(), Random.Shared.Next(1000,9999));
    var gameId = manager.CreateNewGame(player);
    return Results.Created($"/games/{gameId}", new { gameId = gameId });
})
.WithName("CreateGame");

app.MapGet("/games/{gameId}/status", (string gameId, GameManager manager) =>
{
    var table = manager.GetGame(gameId);
    if (table == null) return Results.NotFound("Game not found");
    return Results.Ok(new GameStatusDto(table.players.Count, table.pot, table.communityCards, table.currentPhase.ToString(), table.CurrentPlayer.ConnectionId));
}).WithName("GetGameStatus");


// Make this connect to user database
app.MapPost("/games/join/{gameId}", (string gameId, GameManager manager) =>
{
    var table = manager.GetGame(gameId);
    if (table == null) return Results.NotFound("Game not found");
    try
    {
        var player = new Player(Guid.NewGuid().ToString(), Random.Shared.Next(1000,9999));
        table.AddPlayer(player);
        return Results.Ok();
    }
    catch (Exception ex)
    { 
        return Results.BadRequest(ex.Message);
    }
});

app.MapPost("/games/{gameId}/players/{playerId}/action", (string gameId, string playerId, PlayerActionRequest req, GameManager manager) =>
{
    var table = manager.GetGame(gameId);
    if (table == null) return Results.NotFound("Game not found");

    if (table.CurrentPlayer.ConnectionId != playerId)
        return Results.BadRequest("It's not your turn.");
    try
    {
        switch (req.Action)
        {
            case "bet":
                table.Bet(playerId, req.Amount ?? 0);
                break;
            case "fold":
                table.Fold(playerId);
                break;
            case "check":
                table.Check(playerId);
                break;
            case "call":
                table.Call(playerId);
                break;
        }

        if (table.IsBettingRoundOver())
        {
            table.AdvancePhase();
        }
        else
        {
            table.AdvanceTurn();
        }

        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.Run();