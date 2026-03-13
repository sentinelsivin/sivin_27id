using CodeBase.Domain.Field.Cell;
using UnityEngine;

namespace CodeBase.Domain.Field.View
{
    
    [CreateAssetMenu(fileName = "FieldViewFactory", menuName = "Field/FieldViewFactory", order = 1)]
    public class FieldViewFactory: ScriptableObject
    {
        [SerializeField] private FieldView _fieldViewPrefab;
        [SerializeField] private FieldColumnView _fieldColumnView;
        [SerializeField] private CellView _cellViewPrefab;

        public FieldView Create(Transform parent, int columnsCount, int rowsCount)
        {
            FieldView fieldView = Instantiate(_fieldViewPrefab, parent);
            fieldView.Build(columnsCount, rowsCount);
            return fieldView;
        }
    }
}