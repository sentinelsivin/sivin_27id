using System.Collections.Generic;
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
    }
}
