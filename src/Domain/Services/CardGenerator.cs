using Bingo.src.Domain.Entities;

namespace Bingo.src.Domain.Services
{
    public class CardGenerator
    {
        private readonly Random _random = new Random();

        public BingoCard Generate(int rows = 5, int columns = 5, int minValue = 1, int maxValue = 75)
        {
            int[,] numbers = new int[rows, columns];
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    int num;
                    do
                    {
                        num = _random.Next(minValue, maxValue + 1);
                    } while (ArrayContains(numbers, num, r, c));

                    numbers[r, c] = num;
                }
            }

            return new BingoCard(numbers);
        }

        private bool ArrayContains(int[,] grid, int value, int row, int col)
        {
            for (int r = 0; r <= row; r++)
            {
                int maxCol = (r == row) ? col : grid.GetLength(1);
                for (int c = 0; c < maxCol; c++)
                {
                    if (grid[r, c] == value) return true;
                }
            }
            return false;
        }
    }
}
