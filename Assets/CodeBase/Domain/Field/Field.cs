using System.Collections.Generic;
using CodeBase.Data.PlayerDataComponents;
using CodeBase.Domain.Field.Cell;

namespace CodeBase.Domain.Field
{
    public class Field 
    {
        private readonly Dictionary<PlayerId, PlayerField> _playerFields = new();
        
        public int Rows { get; }
        public int Columns { get; }
        
        public Field(IEnumerable<PlayerId> players, int rows, int columns)
        {
            Rows = rows;
            Columns = columns;

            foreach (var playerId in players)
                _playerFields[playerId] = new PlayerField(playerId, Rows, Columns);
            
        }

        public IEnumerable<PlayerField> PlayerFields { get; set; }

        public PlayerField GetPlayerField(PlayerId playerId) => _playerFields[playerId]; // заглушка

        public bool CanPlaceDice(PlayerId playerId, Domain.Dice.Dice dice, CellPosition position)
        {
            // TODO: валидировать position и пустоту клетки
            return true;
        }

        public void PlaceDice(PlayerId playerId, Domain.Dice.Dice dice, CellPosition position)
        {
            // TODO: реальная установка в PlayerField/Cell
        }
        
        public bool IsFieldFull()
        {
            // TODO: проверка заполненности всех клеток всех полей
            return _playerFields.Count == 0; // заглушка
        }

    }
    
}
