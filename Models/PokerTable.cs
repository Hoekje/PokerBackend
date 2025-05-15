using System.Collections.Generic;
namespace PokerGame.Models
{
    public class PokerTable
    {
        private List<Player> _players = new List<Player>();
        public IReadOnlyList<Player> players { get; private set; }

        public Deck deck { get; private set; } = new Deck();
        public List<Card> communityCards { get; private set; } = new List<Card>();

        public int pot { get; private set; }
        public int currentBet { get; private set; }
        public int dealerPostion { get; private set; } // Tracks the dealer button index

        // Add GamePhase
        public GamePhase currentPhase { get; private set; } = GamePhase.PreDeal;

        public PokerTable()
        {
            players = _players.AsReadOnly();
        }

        public void AddPlayer(Player player)
        {
            _players.Add(player);
        }
        public void RemovePlayer(Player player)
        {
            _players.Remove(player);
        }

        public void StartNewRound()
        {
            communityCards.Clear();
            deck.Reset();
        }
        public void DealFlop()
        {
            communityCards.Add(deck.Draw());
        }
        public enum GamePhase
        {
            PreDeal,
            PreFlop,
            Flop,
            Turn,
            River,
            Showdown
        }
        public override string ToString() => $"Table: {_players.Count} players, Pot: {pot}, Phase: {currentPhase}";
    }

}
