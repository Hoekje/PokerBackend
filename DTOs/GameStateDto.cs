using PokerGame.Models;

namespace PokerGame.DTO
{
    public record GameStatusDto(
        int playerCount,
        int pot,
        List<Card> communityCards,
        string currentPhase,
        string? currentPlayerId
    );

    public record CreateGameResponse(string GameId);
}