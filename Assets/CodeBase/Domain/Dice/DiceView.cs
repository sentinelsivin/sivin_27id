using UnityEngine;

namespace CodeBase.Domain.Dice
{
    public class DiceView : MonoBehaviour
    {
        [SerializeField] private DiceViewContent _diceViewContent;
        [SerializeField] private SpriteRenderer _stateSprite;
        [SerializeField] private SpriteRenderer _pointSprite;
        
        private Dice _dice;

        public void Initialize(Dice dice)
        {
            _dice = dice;
            SetInitSprite();
        }

        private void SetInitSprite()
        {
            SetStateSprite(_dice.diceStateType);
            SetPointSprite(_dice.dicePointType);
        }

        private void SetStateSprite(DiceStateType state) => _stateSprite.sprite = _diceViewContent.GetSprite(state);

        private void SetPointSprite(DicePointType pointType) => _pointSprite.sprite = _diceViewContent.GetSprite(pointType);
    }
}