using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Domain.Field.View
{
    public class FieldColumnView : MonoBehaviour
    {
        [SerializeField] private Transform _container;
        [SerializeField] private CellView _cellPrefab;

        private readonly List<CellView> _cells = new();

        private const int RowsCount = 3;

        public void Build()
        {
            Clear();

            for (int row = 0; row < RowsCount; row++)
            {
                var cellView = Instantiate(_cellPrefab, _container);
                _cells.Add(cellView);
            }
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

        public void PlaceDice(Dice.Dice dice, int row)
        {
            if (row < 0 || row >= _cells.Count)
                throw new ArgumentOutOfRangeException(nameof(row));

            _cells[row].SetDice(dice);
        }
    }
}