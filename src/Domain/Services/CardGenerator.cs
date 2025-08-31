using Bingo.src.Domain.Entities;

namespace Bingo.src.Domain.Services
{
    public class CardGenerator
    {
        private readonly Random _random = new Random();

        public BingoCard Generate(int rows = 5, int columns = 5)
        {
            int[,] numbers = new int[rows, columns];

            // Define the ranges for each column
            int[][] columnRanges = new int[][]
            {
                new int[] {1, 15},    // B
                new int[] {16, 30},   // I
                new int[] {31, 45},   // N
                new int[] {46, 60},   // G
                new int[] {61, 75}    // O
            };

            for (int c = 0; c < columns; c++)
            {
                // Create a list of possible numbers for this column
                List<int> possibleNumbers = new List<int>();
                for (int n = columnRanges[c][0]; n <= columnRanges[c][1]; n++)
                    possibleNumbers.Add(n);

                // Shuffle numbers and pick for each row
                for (int r = 0; r < rows; r++)
                {
                    // Skip center free space
                    if (r == 2 && c == 2)
                    {
                        numbers[r, c] = 0; // FREE space
                        continue;
                    }

                    int index = _random.Next(possibleNumbers.Count);
                    numbers[r, c] = possibleNumbers[index];
                    possibleNumbers.RemoveAt(index);
                }
            }

            return new BingoCard(numbers);
        }
    }
}
