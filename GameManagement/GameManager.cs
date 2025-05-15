using System.Collections.Concurrent;

namespace PokerGame.Models
{
    public class GameManager
    {
        private ConcurrentDictionary<string, PokerTable> _game = new();

        public string CreateNewGame()
        {
            var gameId = Guid.NewGuid().ToString();
            _game.TryAdd(gameId, new PokerTable());
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