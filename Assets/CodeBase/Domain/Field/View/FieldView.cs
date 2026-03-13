using System;
using System.Collections.Generic;
using CodeBase.Domain.Dice;
using CodeBase.Domain.Field.Cell;
using UnityEngine;

namespace CodeBase.Domain.Field.View
{
    public class FieldView : MonoBehaviour
    {
        [SerializeField] private Transform _container;
        [SerializeField] private FieldColumnView _columnPrefab;

        private readonly List<FieldColumnView> _columns = new();

        public void Build(int columnsCount, int rowsCount)
        {
            if (columnsCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(columnsCount));

            if (rowsCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(rowsCount));

            Clear();

            for (int i = 0; i < columnsCount; i++)
            {
                FieldColumnView columnView = Instantiate(_columnPrefab, _container);
                columnView.Build(rowsCount);
                _columns.Add(columnView);
            }
        }

        public void PlaceDiceView(DiceView diceView, CellPosition pos)
        {
            if (diceView == null)
                throw new ArgumentNullException(nameof(diceView));

            if (pos == null)
                throw new ArgumentNullException(nameof(pos));

            if (pos.Col < 0 || pos.Col >= _columns.Count)
                throw new ArgumentOutOfRangeException(nameof(pos));

            _columns[pos.Col].PlaceDiceView(diceView, pos.Row);
        }

        public DiceView RemoveDiceView(CellPosition pos)
        {
            if (pos == null)
                throw new ArgumentNullException(nameof(pos));

            if (pos.Col < 0 || pos.Col >= _columns.Count)
                throw new ArgumentOutOfRangeException(nameof(pos));

            return _columns[pos.Col].RemoveDiceView(pos.Row);
        }

        public CellView GetCell(CellPosition pos)
        {
            if (pos == null)
                throw new ArgumentNullException(nameof(pos));

            if (pos.Col < 0 || pos.Col >= _columns.Count)
                throw new ArgumentOutOfRangeException(nameof(pos));

            return _columns[pos.Col].GetCell(pos.Row);
        }

        public void Clear()
        {
            for (int i = 0; i < _columns.Count; i++)
            {
                if (_columns[i] != null)
                    Destroy(_columns[i].gameObject);
            }

            _columns.Clear();
        }
    }
}