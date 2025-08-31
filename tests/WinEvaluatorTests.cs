using Bingo.src.Domain.Entities;
using Bingo.src.Domain.Services;
using Bingo.src.Domain.Rules;
using Xunit;
using Bingo.src.Domain.Abstractions;

namespace Bingo.tests
{
    public class WinEvaluatorTests
    {
        private BingoCard CreateCard(int[,] numbers)
        {
            return new BingoCard(numbers);
        }

        [Fact]
        public void IsWinning_ShouldReturnTrue_WhenRowComplete()
        {
            int[,] numbers = new int[5, 5]
            {
                {1,2,3,4,5},
                {6,7,8,9,10},
                {11,12,13,14,15},
                {16,17,18,19,20},
                {21,22,23,24,25}
            };

            var card = CreateCard(numbers);
            // Mark first row
            for (int c = 0; c < 5; c++) card.GetNumber(0, c).Mark();

            var evaluator = new WinEvaluator(new IWinRule[] { new RowWinRule() });
            Assert.True(evaluator.IsWinning(card));
        }

        [Fact]
        public void IsWinning_ShouldReturnTrue_WhenColumnComplete()
        {
            int[,] numbers = new int[5, 5]
            {
                {1,2,3,4,5},
                {6,7,8,9,10},
                {11,12,13,14,15},
                {16,17,18,19,20},
                {21,22,23,24,25}
            };

            var card = CreateCard(numbers);
            // Mark first column
            for (int r = 0; r < 5; r++) card.GetNumber(r, 0).Mark();

            var evaluator = new WinEvaluator(new IWinRule[] { new ColumnWinRule() });
            Assert.True(evaluator.IsWinning(card));
        }

        [Fact]
        public void IsWinning_ShouldReturnTrue_WhenDiagonalComplete()
        {
            int[,] numbers = new int[5, 5]
            {
                {1,2,3,4,5},
                {6,7,8,9,10},
                {11,12,13,14,15},
                {16,17,18,19,20},
                {21,22,23,24,25}
            };

            var card = CreateCard(numbers);
            // Mark diagonal
            for (int i = 0; i < 5; i++) card.GetNumber(i, i).Mark();

            var evaluator = new WinEvaluator(new IWinRule[] { new DiagonalWinRule() });
            Assert.True(evaluator.IsWinning(card));
        }

        [Fact]
        public void IsWinning_ShouldReturnFalse_WhenNoWin()
        {
            int[,] numbers = new int[5, 5]
            {
                {1,2,3,4,5},
                {6,7,8,9,10},
                {11,12,13,14,15},
                {16,17,18,19,20},
                {21,22,23,24,25}
            };

            var card = CreateCard(numbers);
            var evaluator = new WinEvaluator(new IWinRule[] { new RowWinRule(), new ColumnWinRule(), new DiagonalWinRule() });

            Assert.False(evaluator.IsWinning(card));
        }
    }
}
