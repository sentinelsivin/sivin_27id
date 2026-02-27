using CodeBase.Data.PlayerDataComponents;
using CodeBase.Domain.Field.Cell;

namespace CodeBase.Domain.Match
{
    public interface IMatchRules
    {
        void ResolveAfterPlacement(Field.Field field, PlayerId placedBy, Dice.Dice placedDice, CellPosition pos);
        MatchResult? TryGetResult(Field.Field field);
        
    }
}