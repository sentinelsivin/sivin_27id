using CodeBase.Data.PlayerDataComponents;

namespace CodeBase.Domain.Field
{
    public class PlayerField
    {
        public PlayerId OwnerId { get; }

        private readonly Cell.Cell[,] _cells;
        private readonly ColumnState[] _columns;

        public PlayerField(PlayerId ownerId, int rows, int columns)
        {
            OwnerId = ownerId;
            _cells = new Cell.Cell[rows, columns];
            _columns = new ColumnState[columns];
        }

        // // доступ к данным
        // public Cell GetCell(int row, int column);
        // public IReadOnlyList<Cell> GetColumn(int column);
        //
        // // логика
        // public bool CanPlaceDice(Dice dice, CellPosition position);
        // public void PlaceDice(Dice dice, CellPosition position);
        //
        // public bool IsColumnFull(int column);

    }
}