using CodeBase.Data.PlayerDataComponents;
using CodeBase.Domain.Field.Cell;

namespace CodeBase.Domain.Match
{
    public interface IMatchRules
    {
        bool CanPlaceDice(Domain.Field.Field field, PlayerId playerId, Domain.Dice.Dice dice, CellPosition pos);
    }
    
}