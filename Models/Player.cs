using Microsoft.Net.Http.Headers;

namespace PokerGame.Models
{
    public class Player
    {
        public string ConnectionId { get; }
        public List<Card> Hand { get; private set; } = new List<Card>();
        public int Chips { get; set; }
        public bool IsReady { get; private set; }
        public bool isActive;

        public bool HasActed { get; set; }
        public bool HasFolded {get; set;}
        public bool HasRaised { get; set; }
        public int CurrentBet { get; set; }


        // Constructor
        public Player(string connectionId, int startingChips)
        {
            ConnectionId = connectionId;
            Chips = startingChips;
            isActive = true;
        }
        public void AddToHand(Card card)
        {
            Hand.Add(card);
        }

        public override string ToString() => 
            $"{ConnectionId}: {Hand.Count} cards, {Chips} chips";
    }
}
