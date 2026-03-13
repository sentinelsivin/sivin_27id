using CodeBase.Domain.Dice;
using UnityEngine;

namespace CodeBase.Domain.Field.Cell
{
    public class CellView : MonoBehaviour
    {
        [SerializeField] private Transform _diceAnchor;

        private DiceView _diceView;

        public bool IsEmpty => _diceView == null;
        public DiceView DiceView => _diceView;

        public void SetDiceView(DiceView diceView)
        {
            if (diceView == null)
                return;

            _diceView = diceView;

            Transform target = _diceAnchor != null ? _diceAnchor : transform;
            Transform diceTransform = diceView.transform;

            diceTransform.SetParent(target, false);
            diceTransform.localPosition = Vector3.zero;
            diceTransform.localRotation = Quaternion.identity;
            diceTransform.localScale = Vector3.one;
        }

        public DiceView RemoveDiceView()
        {
            DiceView result = _diceView;
            _diceView = null;
            return result;
        }

        public void Clear()
        {
            _diceView = null;
        }
    }
}