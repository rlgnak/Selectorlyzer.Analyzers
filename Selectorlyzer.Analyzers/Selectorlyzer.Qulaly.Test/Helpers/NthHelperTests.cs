using FluentAssertions;
using Selectorlyzer.Qulaly.Helpers;

namespace Selectorlyzer.Qulaly.Test.Helpers
{
    public class NthHelperTests
    {
        [Theory]
        [InlineData("even", 0, 2)]
        [InlineData("odd", 1, 2)]
        [InlineData("-7", -7, 0)]
        [InlineData("7", 7, 0)]
        [InlineData("5n", 0, 5)]
        [InlineData("n+7", 7, 1)]
        [InlineData("3n+4", 4, 3)]
        [InlineData("-n+3", 3, -1)]
        [InlineData("n", 0, 1)]
        [InlineData("1", 1, 0)]
        [InlineData("0n+1", 1, 0)]
        public void Should_Parse_Offset_And_Step_From_Nth_Expression(string expression, int expectedOffset, int expectedStep)
        {
            var (offset, step) = NthHelper.GetOffsetAndStep(expression);

            offset.Should().Be(expectedOffset);
            step.Should().Be(expectedStep);
        }

        [Theory]
        [InlineData(0, 0, 2)]
        [InlineData(2, 0, 2)]
        [InlineData(4, 0, 2)]
        [InlineData(1, 1, 2)]
        [InlineData(3, 1, 2)]
        [InlineData(5, 1, 2)]
        [InlineData(7, 7, 0)]
        [InlineData(1, 0, 1)]
        [InlineData(2, 0, 1)]
        [InlineData(3, 0, 1)]
        [InlineData(2, 1, 1)]
        [InlineData(3, 1, 1)]
        [InlineData(4, 1, 1)]
        public void Should_Match_Valid_Index_To_Offset_And_Step(int index, int offset, int step)
        {
            var result = NthHelper.IndexMatchesOffsetAndStep(index, offset, step);
            result.Should().BeTrue();
        }

        [Theory]
        [InlineData(1, 0, 2)]
        [InlineData(3, 0, 2)]
        [InlineData(5, 0, 2)]
        [InlineData(0, 1, 2)]
        [InlineData(2, 1, 2)]
        [InlineData(4, 1, 2)]
        [InlineData(1, 7, 0)]
        [InlineData(0, 1, 1)]
        public void Should_Not_Match_Invalid_Index_To_Offset_And_Step(int index, int offset, int step)
        {
            var result = NthHelper.IndexMatchesOffsetAndStep(index, offset, step);
            result.Should().BeFalse();
        }
    }
}

