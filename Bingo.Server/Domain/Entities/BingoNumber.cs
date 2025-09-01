using System;

namespace Bingo.Server.Domain.Entities
{
    public class BingoNumber
    {
        public int Value { get; }
        public char Letter => GetLetter(Value);
        public bool IsMarked { get; private set; }

        public BingoNumber(int value)
        {
            if (value < 0 || value > 75)
                throw new ArgumentOutOfRangeException(nameof(value), "Bingo number must be between 0 and 75.");

            Value = value;
            IsMarked = false;
        }

        public void Mark() => IsMarked = true;
        public void Unmark() => IsMarked = false;

        private static char GetLetter(int value)
        {
            if (value == 0) return 'F'; // FREE space
            if (value >= 1 && value <= 15) return 'B';
            if (value >= 16 && value <= 30) return 'I';
            if (value >= 31 && value <= 45) return 'N';
            if (value >= 46 && value <= 60) return 'G';
            if (value >= 61 && value <= 75) return 'O';
            throw new ArgumentOutOfRangeException(nameof(value));
        }

        public override string ToString() => $"{Letter}-{Value}";
    }
}
