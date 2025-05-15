using PokerGame.Models;

namespace PokerGame.DTO
{
    public record GameStatusDto(
        int PlayerCount,
        int Pot,
        List<Card> communityCards,
        string currentPhase
    );

    public record CreateGameResponse(string GameId);
}