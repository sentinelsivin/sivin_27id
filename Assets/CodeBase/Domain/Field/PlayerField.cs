using System;
using System.Collections.Generic;
using CodeBase.Data.PlayerDataComponents;
using CodeBase.Domain.Field.Cell;

namespace CodeBase.Domain.Field
{
    public sealed class PlayerField
    {
        public PlayerId OwnerId { get; }

        private readonly Cell.Cell[,] _cells;

        public int RowsCount { get; }
        public int ColumnsCount { get; }

        public PlayerField(PlayerId ownerId, int rows, int columns)
        {
            OwnerId = ownerId;
            RowsCount = rows;
            ColumnsCount = columns;

            _cells = new Cell.Cell[rows, columns];
            for (int r = 0; r < rows; r++)
            for (int c = 0; c < columns; c++)
                _cells[r, c] = new Cell.Cell(r, c);
        }

        public bool CanPlaceDice(CellPosition pos)
        {
            if (!IsInside(pos)) return false;
            return _cells[pos.Row, pos.Col].IsEmpty;
        }

        public void PlaceDice(Dice.Dice dice, CellPosition pos)
        {
            if (!CanPlaceDice(pos))
                throw new InvalidOperationException("Cannot place dice.");

            _cells[pos.Row, pos.Col].PlaceDice(dice);
        }

        public bool TryGetDice(CellPosition pos, out Dice.Dice dice)
        {
            dice = null;
            if (!IsInside(pos)) return false;

            dice = _cells[pos.Row, pos.Col].Dice;
            return dice != null;
        }

        public void Remove(CellPosition pos)
        {
            if (!IsInside(pos)) return;
            // у Cell сейчас нет метода Remove, поэтому просто перезапишем Dice через PlaceDice(null) нельзя.
            // Добавь в Cell: public void Clear() => Dice = null;
            _cells[pos.Row, pos.Col].PlaceDice(null);
        }

        public void RemoveAllInColumnByValue(int col, int value)
        {
            if (col < 0 || col >= ColumnsCount) return;

            for (int row = 0; row < RowsCount; row++)
            {
                var d = _cells[row, col].Dice;
                if (d != null && d.Value == value)
                    _cells[row, col].PlaceDice(null);
            }
        }

        public bool IsFull()
        {
            for (int r = 0; r < RowsCount; r++)
            for (int c = 0; c < ColumnsCount; c++)
                if (_cells[r, c].IsEmpty)
                    return false;
            return true;
        }

        public IReadOnlyList<int> GetColumnDiceValues(int column)
        {
            if (column < 0 || column >= ColumnsCount)
                throw new ArgumentOutOfRangeException(nameof(column));

            var list = new List<int>(RowsCount);
            for (int row = 0; row < RowsCount; row++)
            {
                var dice = _cells[row, column].Dice;
                if (dice != null) list.Add(dice.Value);
            }
            return list;
        }

        private bool IsInside(CellPosition pos)
        {
            if (pos == null) return false;
            return pos.Row >= 0 && pos.Row < RowsCount && pos.Col >= 0 && pos.Col < ColumnsCount;
        }
    }
}