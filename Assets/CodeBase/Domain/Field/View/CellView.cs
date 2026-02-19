namespace CodeBase.Domain.Field.View
{
    public class CellView
    {
        private Cell.Cell _cell;

        public void Bind(Cell.Cell cell)
        {
            _cell = cell;
            UpdateView();
        }

        public void UpdateView()
        {
            
        }
    }
}