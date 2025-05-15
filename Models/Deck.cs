using System;
using System.Collections.Generic;

namespace PokerGame.Models
{
    public class Deck
    {
        private List<Card> _cards = new List<Card>();
        public IReadOnlyList<Card> cards => _cards.AsReadOnly();
        public Deck()
        {
            SetupDeck();
        }

        private void SetupDeck()
        {
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (Rank rank in Enum.GetValues(typeof(Rank)))
                {
                    _cards.Add(new Card(suit, rank));
                }
            }
        }
        public void Shuffle()
        {
            Random rng = new Random();
            int n = _cards.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                (_cards[k], _cards[n]) = (_cards[n], _cards[k]);
            }
        }
        public Card Draw()
        {
            if (_cards.Count == 0)
            {
                throw new InvalidOperationException("Deck is empty");
            }
            Card topCard = _cards[0];
            _cards.RemoveAt(0);
            return topCard;
        }
        public void Reset()
        {
            _cards.Clear();
            SetupDeck();
        }
        public override string ToString()
        {
            return $"Deck: {_cards.Count} cards";
        }
    }
}