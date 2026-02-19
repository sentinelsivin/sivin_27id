using CodeBase.Data.PlayerDataComponents;
using CodeBase.Domain.Field.Cell;

namespace CodeBase.Domain.Match
{
    public class DefaultMatchRules : IMatchRules
    {
        public bool CanPlaceDice(Domain.Field.Field field, PlayerId playerId, Domain.Dice.Dice dice, CellPosition pos)
        {
            // пока просто делегируем Field, потом сюда вынесешь правила
            return field.CanPlaceDice(playerId, dice, pos);
        }
    }
}