using System;

namespace Bingo.src.Domain.Entities
{
    public class BingoCard
    {
        private readonly BingoNumber[,] _grid;

        public int Rows => _grid.GetLength(0);
        public int Columns => _grid.GetLength(1);

        public BingoCard(int[,] numbers)
        {
            int rows = numbers.GetLength(0);
            int cols = numbers.GetLength(1);

            _grid = new BingoNumber[rows, cols];

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    _grid[r, c] = new BingoNumber(numbers[r, c]);
                }
            }
        }

        public void MarkNumber(int value)
        {
            foreach (var number in _grid)
            {
                if (number.Value == value)
                {
                    number.Mark();
                }
            }
        }

        public void UnmarkNumber(int value)
        {
            foreach (var number in _grid)
            {
                if (number.Value == value)
                {
                    number.Unmark();
                }
            }
        }

        public BingoNumber GetNumber(int row, int col) => _grid[row, col];
    }
}
