using System;
using System.Collections.Generic;
using CodeBase.Data.PlayerDataComponents;
using CodeBase.Domain.Field.Cell;
using UnityEngine;

namespace CodeBase.Domain.Field.View
{
    public class FieldView : MonoBehaviour
    {
        [SerializeField] private Transform _container;
        [SerializeField] private FieldColumnView _columnPrefab;

        private readonly List<FieldColumnView> _columns = new();

        public void Build(int columnsCount)
        {
            Clear();

            for (int i = 0; i < columnsCount; i++)
            {
                var columnView = Instantiate(_columnPrefab, _container);
                columnView.Build();
                _columns.Add(columnView);
            }
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

        public void PlaceDice(Dice.Dice dice, CellPosition pos)
        {
            if (pos == null)
                throw new ArgumentNullException(nameof(pos));

            if (pos.Col < 0 || pos.Col >= _columns.Count)
                throw new ArgumentOutOfRangeException(nameof(pos));

            _columns[pos.Col].PlaceDice(dice, pos.Row);
        }
    }
}