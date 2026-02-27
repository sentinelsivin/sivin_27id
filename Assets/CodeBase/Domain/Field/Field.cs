using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Data.PlayerDataComponents;
using CodeBase.Domain.Field.Cell;

namespace CodeBase.Domain.Field
{
    public class Field 
    {
        private readonly Dictionary<PlayerId, PlayerField> _playerFields = new();
        private readonly List<PlayerId> _players;

        private const int Rows = 3;
        private const int Columns = 3;

        public Field(IEnumerable<PlayerId> players)
        {
            _players = players?.ToList() ?? throw new ArgumentNullException(nameof(players));
            foreach (var p in _players)
                _playerFields[p] = new PlayerField(p, Rows, Columns);
        }

        public IReadOnlyList<PlayerId> Players => _players;

        public PlayerField GetPlayerField(PlayerId playerId) => _playerFields[playerId];

        public PlayerId? GetOpponentOf(PlayerId playerId)
        {
            if (_players.Count != 2) return null;
            return _players[0].Equals(playerId) ? _players[1] : _players[0];
        }
        
        public void PlaceDice(PlayerId playerId, Domain.Dice.Dice dice, CellPosition position)
        {
            _playerFields[playerId].PlaceDice(dice, position);
        }

        public bool IsAllFieldsFull()
        {
            foreach (var p in _players)
                if (!_playerFields[p].IsFull())
                    return false;
            return true;
        }
        
    }
    
}
