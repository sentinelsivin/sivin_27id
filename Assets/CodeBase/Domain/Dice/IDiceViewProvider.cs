namespace CodeBase.Domain.Dice
{
    public interface IDiceViewProvider
    {
        DiceView GetView(Dice dice);
    }
}