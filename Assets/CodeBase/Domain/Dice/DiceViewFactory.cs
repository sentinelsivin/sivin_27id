using UnityEngine;

namespace CodeBase.Domain.Dice
{
    [CreateAssetMenu(fileName = "DiceFactory", menuName = "Dice/DiceFactory", order = 1)]
    public class DiceViewFactory : ScriptableObject
    {
        [SerializeField] private DiceView diceViewPrefab;

        public DiceView CreateDice(Dice dice, Transform transform)
        {
            DiceView instance = Instantiate(diceViewPrefab, transform);
            instance.Initialize(dice);

            return instance;
        }
    }
}
