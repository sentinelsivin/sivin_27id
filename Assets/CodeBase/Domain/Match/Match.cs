using System;
using System.Collections.Generic;
using CodeBase.Data.PlayerDataComponents;
using CodeBase.Domain.Field.Cell;

namespace CodeBase.Domain.Match
{
    public sealed class Match
    {
        public event Action<PlayerId> ActivePlayerChanged;             // для UI/индикаторов
        public event Action<PlayerId, Domain.Dice.Dice> DiceChanged;          // для BoardsPresenter
        public event Action<PlayerId, Domain.Dice.Dice, CellPosition> DicePlaced; // для FieldPresenter
        public event Action<PlayerId?> GameEnded;

        private TurnOrder _turnOrder;

        private readonly Dictionary<PlayerId, Domain.Board.Board> _boards = new();
        private readonly Domain.Field.Field _field;
        private readonly IMatchRules _rules;

        public TurnOrder TurnOrder => _turnOrder;
        public bool IsFinished { get; private set; }
        public PlayerId? Winner { get; private set; }

        public Match(
            IReadOnlyList<PlayerId> players,
            int rows,
            int cols,
            PlayerId firstPlayer,
            IMatchRules rules = null)
        {
            if (players == null || players.Count < 2)
                throw new ArgumentException("Match needs at least 2 players.");

            _rules = rules ?? new DefaultMatchRules();
            _turnOrder = new TurnOrder(players, firstPlayer);

            _field = new Domain.Field.Field(players, rows, cols);

            foreach (var p in players)
            {
                var board = new Domain.Board.Board();
                board.DiceChanged += d => DiceChanged?.Invoke(p, d);
                _boards[p] = board;
            }

            ActivePlayerChanged?.Invoke(TurnOrder.ActivePlayer);
        }

        public Domain.Dice.Dice GetDice(PlayerId playerId) => _boards[playerId].Dice;

        public void RollDice(PlayerId playerId)
        {
            EnsureNotFinished();
            EnsureTurnOwner(playerId);

            _boards[playerId].RollDice();
        }

        public void ClearDice(PlayerId playerId) => _boards[playerId].ClearDice();

        public bool CanPlaceDice(PlayerId playerId, CellPosition position)
        {
            EnsureNotFinished();
            EnsureTurnOwner(playerId);

            var dice = _boards[playerId].Dice;
            if (dice == null) return false;

            return _rules.CanPlaceDice(_field, playerId, dice, position);
        }

        public bool TryPlaceDice(PlayerId playerId, CellPosition position)
        {
            EnsureNotFinished();
            EnsureTurnOwner(playerId);

            var dice = _boards[playerId].Dice;
            if (dice == null) return false;

            if (!_rules.CanPlaceDice(_field, playerId, dice, position))
                return false;

            _field.PlaceDice(playerId, dice, position);
            DicePlaced?.Invoke(playerId, dice, position);

            // пример: после установки кубика на поле — очищаем board-кубик
            _boards[playerId].ClearDice();

            // пример завершения: поле заполнено -> конец
            if (_field.IsFieldFull())
            {
                IsFinished = true;
                Winner = null; // пока заглушка
                GameEnded?.Invoke(Winner);
            }

            return true;
        }

        public void EndTurn()
        {
            EnsureNotFinished();
            _turnOrder.Next();
            ActivePlayerChanged?.Invoke(_turnOrder.ActivePlayer);
        }

        private void EnsureTurnOwner(PlayerId playerId)
        {
            if (!_turnOrder.ActivePlayer.Equals(playerId))
                throw new InvalidOperationException($"Not your turn. Active: {_turnOrder.ActivePlayer.Value}");
        }

        private void EnsureNotFinished()
        {
            if (IsFinished)
                throw new InvalidOperationException("Match already finished.");
        }
    }
}
