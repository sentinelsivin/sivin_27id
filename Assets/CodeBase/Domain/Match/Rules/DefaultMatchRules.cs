using System;
using System.Collections.Generic;
using CodeBase.Data.PlayerDataComponents;
using CodeBase.Domain.Field.Cell;
using CodeBase.Domain.Match.Module;

namespace CodeBase.Domain.Match.Rules
{
    public class DefaultMatchRules : IMatchRules
    {
        public void ResolveAfterPlacement(MatchState state, PlayerId placedBy, Dice.Dice placedDice, CellPosition pos)
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state));

            if (placedDice == null)
                return;

            if (pos == null)
                throw new ArgumentNullException(nameof(pos));

            var opponentId = state.GetOpponent(placedBy);
            var opponentField = state.GetField(opponentId);

            opponentField.RemoveAllInColumnByValue(pos.Col, placedDice.Value);
        }

        public MatchResult? TryGetResult(MatchState state)
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state));

            if (!AreAllFieldsFull(state))
                return null;

            if (state.Players.Count != 2)
                throw new InvalidOperationException("DefaultMatchRules assumes exactly 2 players.");

            var firstPlayer = state.Players[0];
            var secondPlayer = state.Players[1];

            var firstScore = ComputeTotalScore(state.GetField(firstPlayer));
            var secondScore = ComputeTotalScore(state.GetField(secondPlayer));

            if (firstScore > secondScore)
                return MatchResult.Win(firstPlayer);

            if (secondScore > firstScore)
                return MatchResult.Win(secondPlayer);

            return MatchResult.Draw();
        }

        private static bool AreAllFieldsFull(MatchState state)
        {
            foreach (var playerId in state.Players)
            {
                if (!state.GetField(playerId).IsFull())
                    return false;
            }

            return true;
        }

        private static int ComputeTotalScore(Field.Field field)
        {
            var total = 0;

            for (int col = 0; col < field.ColumnsCount; col++)
                total += ComputeColumnScore(field.GetColumnDiceValues(col));

            return total;
        }

        // Если в колонке:
        // [3]        -> 3
        // [3,3]      -> 3*2*2 = 12
        // [3,3,3]    -> 3*3*3 = 27
        // [2,2,5]    -> 2*2*2 + 5*1*1 = 8 + 5 = 13
        private static int ComputeColumnScore(IReadOnlyList<int> values)
        {
            var counts = new Dictionary<int, int>();

            for (int i = 0; i < values.Count; i++)
            {
                var value = values[i];

                counts.TryGetValue(value, out var count);
                counts[value] = count + 1;
            }

            var sum = 0;

            foreach (var pair in counts)
            {
                var value = pair.Key;
                var count = pair.Value;

                sum += value * count * count;
            }

            return sum;
        }
    }
}