using System;
using Random = System.Random;

namespace CodeBase.Domain.Dice
{
    public class Dice
    {
        public DiceStateType diceStateType;
        public DicePointType dicePointType;
        
        private static Random _random = new Random();
        
        public Dice Roll()
        {
            Dice data = new Dice();
            
            data.dicePointType = GetRandomEnum<DicePointType>();
            data.diceStateType = GetRandomEnum<DiceStateType>();
            
            return data;
        }
        
        public static T GetRandomEnum<T>() where T : Enum
        {
            Array values = Enum.GetValues(typeof(T));
            return (T)values.GetValue(_random.Next(values.Length));
        }
    }

    public enum DiceStateType
    {
        Green,
        Blue,
        Yellow,
    }

    public enum DicePointType
    {
        One,
        Two,
        Three,
        Four, 
        Five,
        Six
    }
}

