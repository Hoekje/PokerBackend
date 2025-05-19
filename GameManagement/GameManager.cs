using System.Collections.Concurrent;

namespace PokerGame.Models
{
    public class GameManager
    {
        private ConcurrentDictionary<string, PokerTable> _game = new();

        public string CreateNewGame(Player hostPlayer)
        {
            var gameId = Guid.NewGuid().ToString();
            _game.TryAdd(gameId, new PokerTable(gameId, hostPlayer));
            return gameId;
        }

        public PokerTable? GetGame(string gameId)
        {
            if (_game.TryGetValue(gameId, out var table))
            {
                return table;
            }
            else
            {
                return null;
            }
        }
        public bool RemoveGame(string gameId)
        {
            return _game.TryRemove(gameId, out _);
        }

    }
}