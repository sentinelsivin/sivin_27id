using System;
using System.Collections.Generic;
using CodeBase.Data.PlayerDataComponents;
using CodeBase.Domain.Field;

namespace CodeBase.Domain.Match.Module
{
    public class MatchState
    {
        public IReadOnlyList<PlayerId> Players { get; }
        public TurnOrder TurnOrder { get; }
        private readonly Dictionary<PlayerId, Board.Board> _boards = new();
        private readonly Field.Field _field;

        public MatchState(IReadOnlyList<PlayerId> players, PlayerId firstPlayer)
        {
            Players = players ?? throw new ArgumentNullException(nameof(players));
            if (Players.Count < 2) throw new ArgumentException("Match needs at least 2 players.");

            TurnOrder = new TurnOrder(Players, firstPlayer);
            _field = new Field.Field(Players);

            foreach (var p in Players)
                _boards[p] = new Board.Board();
        }

        public PlayerField GetField(PlayerId id) => _field.GetPlayerField(id);
        public Field.Field Field => _field;
        public Board.Board GetBoard(PlayerId id) => _boards[id];

        public PlayerId GetOpponent(PlayerId id)
            => Players[0].Equals(id) ? Players[1] : Players[0];
    }
}