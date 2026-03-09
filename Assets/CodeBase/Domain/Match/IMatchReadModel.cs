using System;
using System.Collections.Generic;
using CodeBase.Data.PlayerDataComponents;
using CodeBase.Domain.Field;
using CodeBase.Domain.Field.Cell;

namespace CodeBase.Domain.Match
{
    public interface IMatchReadModel
    {
        IReadOnlyList<PlayerId> Players { get; }
        PlayerId ActivePlayer { get; }

        Field.Field GetField(PlayerId id);
        Board.Board GetBoard(PlayerId id);
        Dice.Dice GetDice(PlayerId id);

        event Action<PlayerId> ActivePlayerChanged;
        event Action<PlayerId, Dice.Dice> DiceChanged;
        event Action<PlayerId, Dice.Dice, CellPosition> DicePlaced;
        event Action<PlayerId?> GameEnded;
    }
}