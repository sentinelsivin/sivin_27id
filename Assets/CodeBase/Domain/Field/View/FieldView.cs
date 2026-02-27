using System.Collections.Generic;
using CodeBase.Domain.Field.Cell;
using UnityEngine;

namespace CodeBase.Domain.Field.View
{
    public class FieldView : MonoBehaviour
    {
        [SerializeField] private List<CellView> _cellPrefab;
        [SerializeField] private Transform _container;

        public void Build(IEnumerable<Domain.Field.Field> heroes)
        {
            
        }

        public void Clear()
        {
            
        }

        public void PlaceDice(Dice.Dice dice, CellPosition pos)
        {
            throw new System.NotImplementedException();
        }
    }
}
