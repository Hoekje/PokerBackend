namespace PokerGame.Models
{
    public class Player
    {
        // Core properties (no logic)
        public string ConnectionId { get; }
        public List<Card> Hand { get; private set; } = new List<Card>();
        public int Chips { get; private set; }
        public bool IsActive { get; private set; }
        public int CurrentBet { get; private set; }

        // Constructor
        public Player(string connectionId, int startingChips)
        {
            ConnectionId = connectionId;
            Chips = startingChips;
            IsActive = true;
        }

        // Method stubs (implement later)
        public void Fold()
        {
            IsActive = false;
        }
        // public void Bet(int amount)
        // {

            // }
        public void AddToHand(Card card)
        {
            Hand.Add(card);
        }
        // public void ResetForNewRound()
        // {
            
        // }

        public override string ToString() => 
            $"{ConnectionId}: {Hand.Count} cards, {Chips} chips";
    }
}
