using System;

namespace CodeBase.Domain.Board
{
    public class Board
    {
        public event Action<Dice.Dice> DiceChanged;

        public Dice.Dice Dice { get; private set; }

        public void RollDice()
        {
            Dice = Dice.Roll(); 
            DiceChanged?.Invoke(Dice);
        }

        public void ClearDice()
        {
            Dice = null;
            DiceChanged?.Invoke(null);
        }
        
        
        
    }
}