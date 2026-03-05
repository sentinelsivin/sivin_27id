using System;
using System.Collections.Generic;
using CodeBase.Data.PlayerDataComponents;
using CodeBase.Domain.Field;
using CodeBase.Domain.Field.Cell;
using CodeBase.Domain.Match.Module;

namespace CodeBase.Domain.Match
{
    public class Match : IMatchReadModel
    {
        public event Action<PlayerId> ActivePlayerChanged;
        public event Action<PlayerId, Dice.Dice> DiceChanged;
        public event Action<PlayerId, Dice.Dice, CellPosition> DicePlaced;
        public event Action<PlayerId?> GameEnded;
        
        public IReadOnlyList<PlayerId> Players => State.Players;
        public PlayerId ActivePlayer => State.TurnOrder.ActivePlayer;
        public PlayerField GetField(PlayerId id) => State.GetField(id);
        public Board.Board GetBoard(PlayerId id) => State.GetBoard(id);
        public TurnOrder TurnOrder => State.TurnOrder;
        
        public MatchState State { get; }
        public PlayerId? Winner { get; private set; }
        public bool IsFinished { get; private set; }
        
        private readonly IMatchRules _rules;
        
        public Match(MatchState state, IMatchRules rules)
        {
            State = state ?? throw new ArgumentNullException(nameof(state));
            _rules = rules ?? throw new ArgumentNullException(nameof(rules));
            
            foreach (var p in State.Players)
            {
                var board = State.GetBoard(p);
                board.DiceChanged += d => DiceChanged?.Invoke(p, d);
            }

            ActivePlayerChanged?.Invoke(State.TurnOrder.ActivePlayer);
        }
        
        public PlayerField Get(PlayerId id) => State.GetField(id);
        public Dice.Dice GetDice(PlayerId playerId) => State.GetBoard(playerId).Dice;

        public void RollDice(PlayerId playerId)
        {
            EnsureNotFinished();
            EnsureTurnOwner(playerId);
            State.GetBoard(playerId).RollDice();
        }

        public bool TryPlaceDice(PlayerId playerId, CellPosition position)
        {
            EnsureNotFinished();
            EnsureTurnOwner(playerId);

            var board = State.GetBoard(playerId);
            var dice = board.Dice;
            if (dice == null) return false;

            if (!State.GetField(playerId).CanPlaceDice(position))
                return false;

            State.Field.PlaceDice(playerId, dice, position);
            _rules.ResolveAfterPlacement(State.Field, playerId, dice, position);

            DicePlaced?.Invoke(playerId, dice, position);
            board.ClearDice();

            var result = _rules.TryGetResult(State.Field);
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
            State.TurnOrder.Next();
            ActivePlayerChanged?.Invoke(State.TurnOrder.ActivePlayer);
        }

        private void EnsureTurnOwner(PlayerId playerId)
        {
            if (!State.TurnOrder.ActivePlayer.Equals(playerId))
                throw new InvalidOperationException($"Not your turn. Active: {State.TurnOrder.ActivePlayer.Value}");
        }

        private void EnsureNotFinished()
        {
            if (IsFinished)
                throw new InvalidOperationException("Match already finished.");
        }
    }
}
