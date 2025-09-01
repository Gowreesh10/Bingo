namespace Bingo.Server.Domain.Services
{
    public class NumberDrawer
    {
        private readonly List<int> _numbers;
        private readonly Random _random = new Random();

        public NumberDrawer(int min = 1, int max = 75)
        {
            _numbers = new List<int>();
            for (int i = min; i <= max; i++)
            {
                _numbers.Add(i);
            }
        }

        public int? Draw()
        {
            if (_numbers.Count == 0) return null;

            int index = _random.Next(_numbers.Count);
            int number = _numbers[index];
            _numbers.RemoveAt(index);
            return number;
        }

        public bool HasNumbersLeft() => _numbers.Count > 0;
    }
}
