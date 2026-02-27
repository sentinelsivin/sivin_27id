using System;
using System.Collections.Generic;
using CodeBase.Data.PlayerDataComponents;
using CodeBase.Domain.Field;
using CodeBase.Domain.Field.Cell;

namespace CodeBase.Domain.Match
{
    public sealed class Match
    {
        public event Action<PlayerId> ActivePlayerChanged;             // для UI/индикаторов
        public event Action<PlayerId, Domain.Dice.Dice> DiceChanged;          // для BoardsPresenter
        public event Action<PlayerId, Domain.Dice.Dice, CellPosition> DicePlaced; // для FieldPresenter
        public event Action<PlayerId?> GameEnded;
        public TurnOrder TurnOrder => _turnOrder;

        private TurnOrder _turnOrder;
        private readonly Dictionary<PlayerId, Domain.Board.Board> _boards = new();
        private readonly Domain.Field.Field _field;
        private readonly IMatchRules _rules;

        private IReadOnlyList<PlayerId> _players;

        public bool IsFinished { get; private set; }
        public PlayerId? Winner { get; private set; }

        public Match(
            IReadOnlyList<PlayerId> players,
            PlayerId firstPlayer,
            IMatchRules rules = null)
        {
            _players = players;
            
            if (_players == null || _players.Count < 2)
                throw new ArgumentException("Match needs at least 2 players.");

            _rules = rules ?? new DefaultMatchRules();
            _turnOrder = new TurnOrder(_players, firstPlayer);

            _field = new Domain.Field.Field(_players);

            foreach (var p in _players)
            {
                var board = new Domain.Board.Board();
                board.DiceChanged += d => DiceChanged?.Invoke(p, d);
                _boards[p] = board;
            }

            ActivePlayerChanged?.Invoke(TurnOrder.ActivePlayer);
        }

        public PlayerField Get(PlayerId id) => _field.GetPlayerField(id);

        public bool CanPlaceDice(PlayerId id, Dice.Dice dice, CellPosition pos)
            => Get(id).CanPlaceDice(pos);

        public void PlaceDice(PlayerId id, Dice.Dice dice, CellPosition pos)
            => Get(id).PlaceDice(dice, pos);

        public PlayerId GetOpponent(PlayerId id) => _players[0].Equals(id) ? _players[1] : _players[0];
        
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

            return _field.GetPlayerField(playerId).CanPlaceDice(position);
        }
        
        public bool TryPlaceDice(PlayerId playerId, CellPosition position)
        {
            EnsureNotFinished();
            EnsureTurnOwner(playerId);

            var dice = _boards[playerId].Dice;
            if (dice == null) return false;

            if (!_field.GetPlayerField(playerId).CanPlaceDice(position))
                return false;

            _field.PlaceDice(playerId, dice, position);
            _rules.ResolveAfterPlacement(_field, playerId, dice, position);

            DicePlaced?.Invoke(playerId, dice, position);
            _boards[playerId].ClearDice();

            var result = _rules.TryGetResult(_field);
            if (result.HasValue)
            {
                IsFinished = true;
                Winner = result.Value.Winner;
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
