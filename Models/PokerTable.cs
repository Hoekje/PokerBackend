using System.Collections.Generic;
namespace PokerGame.Models
{
    public class PokerTable
    {
        public string GameId { get; set; }
        private int minPlayer = 2;
        public bool gameStarted = false;
        private List<Player> _players = new List<Player>();
        public IReadOnlyList<Player> players { get; private set; }

        public Deck deck { get; private set; } = new Deck();
        public List<Card> communityCards { get; private set; } = new List<Card>();

        public int pot { get; private set; }
        public int currentBet { get; private set; }
        public int dealerPostion { get; private set; } // Tracks the dealer button index

        // Add GamePhase
        public GamePhase currentPhase { get; private set; } = GamePhase.WaitingForPlayers;

        public int CurrentTurnIndex { get; set; } = 0; // Index in Players list

        public Player CurrentPlayer => players[CurrentTurnIndex];

        public PokerTable(string gameId, Player hostPlayer)
        {
            players = _players.AsReadOnly();
            GameId = gameId;
            _players.Add(hostPlayer);
        }

        public void AdvancePhase()
        {
            switch (currentPhase)
            {
                case GamePhase.PreDeal:
                    currentPhase = GamePhase.PreFlop;
                    break;
                case GamePhase.PreFlop:
                    // Play the three cards  
                    currentPhase = GamePhase.Flop;
                    break;
                case GamePhase.Flop:
                    currentPhase = GamePhase.Turn;
                    break;
                case GamePhase.Turn:
                    currentPhase = GamePhase.River;
                    break;
                case GamePhase.River:
                    currentPhase = GamePhase.Showdown;
                    break;
                case GamePhase.Showdown:
                    currentPhase = GamePhase.Finished;
                    break;
            }
        }
        public void AdvanceTurn()
        {
            int totalPlayers = players.Count;
            do
            {
                CurrentTurnIndex = (CurrentTurnIndex + 1) % totalPlayers;
            }
            while (players[CurrentTurnIndex].HasFolded || !players[CurrentTurnIndex].isActive);

        }
        public void StartNewRound()
        {
            communityCards.Clear();
            deck.Reset();
        }
        public void Bet(string playerId, int amount)
        {
            var player = players.FirstOrDefault(p => p.ConnectionId == playerId);
            if (player == null) throw new Exception("Player no found");
            if (amount > player.Chips) throw new Exception("Insufficient chips");

            player.Chips -= amount;
            pot += amount;
            player.CurrentBet += amount;
            player.HasActed = true;    
        }

        public void Fold(string playerId)
        {
            var player = players.FirstOrDefault(p => p.ConnectionId == playerId);
            if (player == null) throw new Exception("Player no found");
            player.HasFolded = true;
            player.HasActed = true;

        }
        public void Check(string playerId)
        {
            var player = players.FirstOrDefault(p => p.ConnectionId == playerId);
            if (player == null) throw new Exception("Player no found");
            player.HasActed = true;
        }
        public void Call(string playerId)
        {
            var player = players.FirstOrDefault(p => p.ConnectionId == playerId);
            if (player == null) throw new Exception("Player no found");
            if (currentBet > player.Chips) throw new Exception("Insufficient chips");

            player.Chips -= currentBet;
            pot += currentBet;
            player.CurrentBet += currentBet;
            player.HasActed = true; 

        }
        public bool IsBettingRoundOver()
        {
            if (players.All(p => p.HasActed || p.HasFolded))
            {
                AdvancePhase();
                ResetPlayerActions();
                return true;
            }
            return false;
        }
        public void ResetPlayerActions()
        {
            foreach (var player in players)
            {
                player.HasActed = false;
            }
        }
        public void AddPlayer(Player player)
        {
            _players.Add(player);
            if (_players.Count >= minPlayer && !gameStarted)
            {
                StartGame();
            }
        }
        public void StartGame()
        {
            gameStarted = true;
            currentPhase = GamePhase.PreDeal;
            deck.Shuffle();
            DealCards(2);
        }
        public void DealCards(int amount)
        {
            foreach (var player in players)
            {
                for (int i = 0; i >= amount; i++)
                {
                    player.Hand.Add(deck.Draw());
                }
            }
        }
        public void RemovePlayer(Player player)
        {
            _players.Remove(player);
        }
        public void DealFlop()
        {
            communityCards.Add(deck.Draw());
        }
        public enum GamePhase
        {
            WaitingForPlayers,
            PreDeal,
            PreFlop,
            Flop,
            Turn,
            River,
            Showdown,
            Finished
        }
        public override string ToString() => $"Table: {_players.Count} players, Pot: {pot}, Phase: {currentPhase}";
    }

}
