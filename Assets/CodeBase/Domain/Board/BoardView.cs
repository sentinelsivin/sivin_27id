using CodeBase.Domain.Dice;
using UnityEngine;

namespace CodeBase.Domain.Board
{
    public class BoardView : MonoBehaviour
    {
        [SerializeField] private RectTransform _diceRoot;
        [SerializeField] private DiceView _diceView;
        [SerializeField] private DiceViewFactory _diceViewFactory;
        
        public void EnsureDiceView(Dice.Dice dice)
        { 
            if (dice == null)
            {
                if (_diceView != null)
                    Destroy(_diceView.gameObject);

                _diceView = null;
                return;
            }

            if (_diceView != null)
                Destroy(_diceView.gameObject);

            _diceView = _diceViewFactory.CreateDice(dice, _diceRoot);
        }
    }
}
