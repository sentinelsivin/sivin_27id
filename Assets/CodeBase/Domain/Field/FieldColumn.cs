using System;
using System.Collections.Generic;

namespace CodeBase.Domain.Field
{
    public class FieldColumn
    {
        private readonly Cell.Cell[] _cells;

        public int Index { get; }
        public int RowsCount => _cells.Length;

        public FieldColumn(int index, int rows)
        {
            if (rows <= 0)
                throw new ArgumentOutOfRangeException(nameof(rows));

            Index = index;
            _cells = new Cell.Cell[rows];

            for (int row = 0; row < rows; row++)
                _cells[row] = new Cell.Cell(row, index);
        }

        public bool CanPlaceDice(int row)
        {
            if (!IsInside(row))
                return false;

            return _cells[row].IsEmpty;
        }

        public void PlaceDice(Dice.Dice dice, int row)
        {
            if (dice == null)
                throw new ArgumentNullException(nameof(dice));

            if (!CanPlaceDice(row))
                throw new InvalidOperationException("Cannot place dice.");

            _cells[row].PlaceDice(dice);
        }

        public bool TryGetDice(int row, out Dice.Dice dice)
        {
            dice = null;

            if (!IsInside(row))
                return false;

            dice = _cells[row].Dice;
            return dice != null;
        }

        public void Remove(int row)
        {
            if (!IsInside(row))
                return;

            _cells[row].Clear();
        }

        public void RemoveAllByValue(int value)
        {
            for (int row = 0; row < _cells.Length; row++)
            {
                var dice = _cells[row].Dice;
                if (dice != null && dice.Value == value)
                    _cells[row].Clear();
            }
        }

        public IReadOnlyList<int> GetDiceValues()
        {
            var result = new List<int>(_cells.Length);

            for (int row = 0; row < _cells.Length; row++)
            {
                var dice = _cells[row].Dice;
                if (dice != null)
                    result.Add(dice.Value);
            }

            return result;
        }

        public bool IsFull()
        {
            for (int row = 0; row < _cells.Length; row++)
            {
                if (_cells[row].IsEmpty)
                    return false;
            }

            return true;
        }

        private bool IsInside(int row)
        {
            return row >= 0 && row < _cells.Length;
        }
    }
}