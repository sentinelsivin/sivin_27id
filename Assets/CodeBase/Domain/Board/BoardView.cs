using CodeBase.Domain.Dice;
using UnityEngine;

namespace CodeBase.Domain.Board
{
    public class BoardView : MonoBehaviour
    {
        [SerializeField] private RectTransform _diceRoot;
        [SerializeField] private DiceViewFactory _diceViewFactory;
        
        private DiceView _diceView;

        public void EnsureDiceView(Dice.Dice dice)
        {
            if (dice == null)
            {
                ClearDiceView();
                return;
            }

            ClearDiceView();
            _diceView = _diceViewFactory.CreateDice(dice, _diceRoot);
        }
        
        private void ClearDiceView()
        {
            if (_diceView == null)
                return;

            Destroy(_diceView.gameObject);
            _diceView = null;
        }
    }
}