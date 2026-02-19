using CodeBase.Domain.Dice;
using CodeBase.Domain.Field;

namespace CodeBase.Data.PlayerDataComponents
{
    public class PlayerRuntimeState 
    {
        public Dice ActiveDice { get; set; }
        public Field Field { get; set; }
    }
}
