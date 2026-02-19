namespace CodeBase.Domain.Field.Cell
{
    public class Cell 
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public Domain.Dice.Dice Dice { get; private set; }

        public bool IsEmpty => Dice == null;

        public Cell(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public void PlaceDice(Domain.Dice.Dice dice) => Dice = dice;
    }
}
