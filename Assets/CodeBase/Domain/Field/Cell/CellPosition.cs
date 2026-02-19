namespace CodeBase.Domain.Field.Cell
{
    public class CellPosition
    {
        public int Row { get; }
        public int Col { get; }

        public CellPosition(int row, int col)
        {
            Row = row;
            Col = col;
        }
    }
}