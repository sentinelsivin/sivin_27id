using System;
using System.Collections.Generic;
using CodeBase.Data.PlayerDataComponents;
using CodeBase.Domain.Field;
using CodeBase.Domain.Field.Cell;

namespace CodeBase.Domain.Match
{
    public class DefaultMatchRules : IMatchRules
    {
        
        public void ResolveAfterPlacement(Field.Field field, PlayerId placedBy, Dice.Dice placedDice, CellPosition pos)
        {
            if (field == null) throw new ArgumentNullException(nameof(field));
            if (placedDice == null) return;

            var opponent = field.GetOpponentOf(placedBy);
            if (opponent == null) return;

            // ВЫБИВАНИЕ:
            // классический вариант: выбиваем у противника ВСЕ дайсы с тем же значением в ТОМ ЖЕ СТОЛБЦЕ
            var oppField = field.GetPlayerField(opponent.Value);
            oppField.RemoveAllInColumnByValue(pos.Col, placedDice.Value);

            // Если тебе нужно "в той же клетке (row+col)" — замени на:
            // if (oppField.TryGetDice(pos, out var d) && d.Value == placedDice.Value) oppField.Remove(pos);
        }
        

        public MatchResult? TryGetResult(Field.Field field)
        {
            if (field == null) throw new ArgumentNullException(nameof(field));

            // конец игры: оба поля заполнены
            if (!field.IsAllFieldsFull())
                return null;

            var players = field.Players; // IReadOnlyList<PlayerId>
            if (players.Count != 2)
                throw new InvalidOperationException("DefaultMatchRules assumes exactly 2 players.");

            var a = players[0];
            var b = players[1];

            int scoreA = ComputeTotalScore(field.GetPlayerField(a));
            int scoreB = ComputeTotalScore(field.GetPlayerField(b));

            if (scoreA > scoreB) return MatchResult.Win(a);
            if (scoreB > scoreA) return MatchResult.Win(b);
            return MatchResult.Draw();
        }

        private static int ComputeTotalScore(PlayerField playerField)
        {
            int total = 0;
            for (int col = 0; col < playerField.ColumnsCount; col++)
                total += ComputeColumnScore(playerField.GetColumnDiceValues(col));
            return total;
        }

        // Скоринг с мультипликатором одинаковых значений в столбце:
        // если в столбце два "3", то вклад = 3*2*2 = 12 (потому что каждый 3 умножается на 2)
        // если три "3" -> 3*3*3 = 27
        private static int ComputeColumnScore(IReadOnlyList<int> values)
        {
            // values содержит только заполненные клетки (без пустых)
            // группируем по значению
            var counts = new Dictionary<int, int>();
            for (int i = 0; i < values.Count; i++)
            {
                int v = values[i];
                counts.TryGetValue(v, out int c);
                counts[v] = c + 1;
            }

            int sum = 0;
            foreach (var kv in counts)
            {
                int v = kv.Key;
                int c = kv.Value;
                sum += v * c * c;
            }

            return sum;
        }
    }
}