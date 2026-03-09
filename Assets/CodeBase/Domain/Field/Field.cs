using System;
using System.Collections.Generic;
using CodeBase.Data.PlayerDataComponents;
using CodeBase.Domain.Field.Cell;

namespace CodeBase.Domain.Field
{
    public class Field
    {
        private const int Rows = 3;
        private const int Columns = 3;

        private readonly FieldColumn[] _columns;

        public PlayerId OwnerId { get; }

        public int RowsCount => Rows;
        public int ColumnsCount => Columns;

        public Field(PlayerId ownerId)
        {
            OwnerId = ownerId;
            _columns = new FieldColumn[Columns];

            for (int i = 0; i < Columns; i++)
                _columns[i] = new FieldColumn(i, Rows);
        }

        public FieldColumn GetColumn(int columnIndex)
        {
            if (columnIndex < 0 || columnIndex >= ColumnsCount)
                throw new ArgumentOutOfRangeException(nameof(columnIndex));

            return _columns[columnIndex];
        }

        public bool CanPlaceDice(CellPosition pos)
        {
            if (!IsInside(pos))
                return false;

            return _columns[pos.Col].CanPlaceDice(pos.Row);
        }

        public void PlaceDice(Dice.Dice dice, CellPosition pos)
        {
            if (dice == null)
                throw new ArgumentNullException(nameof(dice));

            if (!CanPlaceDice(pos))
                throw new InvalidOperationException("Cannot place dice.");

            _columns[pos.Col].PlaceDice(dice, pos.Row);
        }

        public bool TryGetDice(CellPosition pos, out Dice.Dice dice)
        {
            dice = null;

            if (!IsInside(pos))
                return false;

            return _columns[pos.Col].TryGetDice(pos.Row, out dice);
        }

        public void Remove(CellPosition pos)
        {
            if (!IsInside(pos))
                return;

            _columns[pos.Col].Remove(pos.Row);
        }

        public void RemoveAllInColumnByValue(int column, int value)
        {
            if (column < 0 || column >= ColumnsCount)
                return;

            _columns[column].RemoveAllByValue(value);
        }

        public IReadOnlyList<int> GetColumnDiceValues(int column)
        {
            if (column < 0 || column >= ColumnsCount)
                throw new ArgumentOutOfRangeException(nameof(column));

            return _columns[column].GetDiceValues();
        }

        public bool IsFull()
        {
            for (int i = 0; i < _columns.Length; i++)
            {
                if (!_columns[i].IsFull())
                    return false;
            }

            return true;
        }

        private bool IsInside(CellPosition pos)
        {
            if (pos == null)
                return false;

            return pos.Row >= 0 && pos.Row < RowsCount &&
                   pos.Col >= 0 && pos.Col < ColumnsCount;
        }
    }
}