using Bingo.src.Domain.Abstractions;
using Bingo.src.Domain.Entities;

namespace Bingo.src.Domain.Rules
{

    public class RowWinRule : IWinRule
    {
        public bool IsWinning(BingoCard card)
        {
            for (int row = 0; row < card.Rows; row++)
            {
                bool rowComplete = true;
                for (int col = 0; col < card.Columns; col++)
                {
                    if (!card.GetNumber(row, col).IsMarked)
                    {
                        rowComplete = false;
                        break;
                    }
                }
                if (rowComplete)
                    return true;
            }
            return false;
        }
    }

    public class ColumnWinRule : IWinRule
    {
        public bool IsWinning(BingoCard card)
        {
            for (int col = 0; col < card.Columns; col++)
            {
                bool colComplete = true;
                for (int row = 0; row < card.Rows; row++)
                {
                    if (!card.GetNumber(row, col).IsMarked)
                    {
                        colComplete = false;
                        break;
                    }
                }
                if (colComplete)
                    return true;
            }
            return false;
        }
    }

    public class DiagonalWinRule : IWinRule
    {
        public bool IsWinning(BingoCard card)
        {
            bool diag1 = true;
            bool diag2 = true;

            for (int i = 0; i < card.Rows && i < card.Columns; i++)
            {
                if (!card.GetNumber(i, i).IsMarked)
                    diag1 = false;

                if (!card.GetNumber(i, card.Columns - 1 - i).IsMarked)
                    diag2 = false;
            }

            return diag1 || diag2;
        }
    }
}
