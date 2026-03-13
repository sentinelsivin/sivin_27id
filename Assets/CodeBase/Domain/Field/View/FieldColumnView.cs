using System;
using System.Collections.Generic;
using CodeBase.Domain.Dice;
using CodeBase.Domain.Field.Cell;
using UnityEngine;

namespace CodeBase.Domain.Field.View
{
    public class FieldColumnView : MonoBehaviour
    {
        [SerializeField] private Transform _container;
        [SerializeField] private CellView _cellPrefab;

        private readonly List<CellView> _cells = new();

        public void Build(int rowsCount)
        {
            if (rowsCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(rowsCount));

            Clear();

            for (int i = 0; i < rowsCount; i++)
            {
                CellView cellView = Instantiate(_cellPrefab, _container);
                _cells.Add(cellView);
            }
        }

        public void PlaceDiceView(DiceView diceView, int row)
        {
            if (row < 0 || row >= _cells.Count)
                throw new ArgumentOutOfRangeException(nameof(row));

            _cells[row].SetDiceView(diceView);
        }

        public DiceView RemoveDiceView(int row)
        {
            if (row < 0 || row >= _cells.Count)
                throw new ArgumentOutOfRangeException(nameof(row));

            return _cells[row].RemoveDiceView();
        }

        public CellView GetCell(int row)
        {
            if (row < 0 || row >= _cells.Count)
                throw new ArgumentOutOfRangeException(nameof(row));

            return _cells[row];
        }

        public void Clear()
        {
            for (int i = 0; i < _cells.Count; i++)
            {
                if (_cells[i] != null)
                    Destroy(_cells[i].gameObject);
            }

            _cells.Clear();
        }
    }
}