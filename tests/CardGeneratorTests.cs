using Bingo.src.Domain.Entities;
using Bingo.src.Domain.Services;
using Xunit;
using System.Linq;

namespace Bingo.tests
{
    public class CardGeneratorTests
    {
        [Fact]
        public void Generate_ShouldReturnCardWithCorrectDimensions()
        {
            var generator = new CardGenerator();
            var card = generator.Generate(5, 5);

            Assert.Equal(5, card.Rows);
            Assert.Equal(5, card.Columns);
        }

        [Fact]
        public void Generate_ShouldReturnCardWithUniqueNumbersPerColumn()
        {
            var generator = new CardGenerator();
            var card = generator.Generate(5, 5);

            int[][] columnRanges = new int[][]
            {
                new int[] {1, 15},    // B
                new int[] {16, 30},   // I
                new int[] {31, 45},   // N
                new int[] {46, 60},   // G
                new int[] {61, 75}    // O
            };

            for (int c = 0; c < card.Columns; c++)
            {
                var numbersInColumn = Enumerable.Range(0, card.Rows)
                                                .Where(r => !(r == 2 && c == 2)) // skip free space
                                                .Select(r => card.GetNumber(r, c).Value)
                                                .ToList();

                // Check all numbers are unique in this column
                Assert.Equal(numbersInColumn.Count, numbersInColumn.Distinct().Count());

                // Check all numbers are in the correct column range
                foreach (var val in numbersInColumn)
                {
                    Assert.InRange(val, columnRanges[c][0], columnRanges[c][1]);
                }
            }
        }

        [Fact]
        public void Generate_ShouldHaveFreeSpaceInCenter()
        {
            var generator = new CardGenerator();
            var card = generator.Generate(5, 5);

            Assert.Equal(0, card.GetNumber(2, 2).Value); // center free space
        }
    }
}
