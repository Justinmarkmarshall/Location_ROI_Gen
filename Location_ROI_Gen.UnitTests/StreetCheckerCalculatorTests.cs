using FluentAssertions;
using Location_ROI_Gen.Calculator;

namespace Location_ROI_Gen.UnitTests
{
    public class StreetCheckerCalculatorTests
    {
        private readonly StreetCheckerCalculator _streetCheckerCalculator;
        public StreetCheckerCalculatorTests()
        {
            _streetCheckerCalculator = new StreetCheckerCalculator();
        }

        [Theory]
        [InlineData(50, 100, 50)]
        [InlineData(25, 100, 25)]
        [InlineData(75, 100, 75)]
        [InlineData(35, 203, 17)]

        public void CalculatePerce_CalculatesPercentage(int number, int total, int expected)
        {
            // arrange

            // act
            var result = _streetCheckerCalculator.CalculatePercentage(number, total);

            // assert
            result.Should().Be(expected);
        }

        [Fact]
        public void CalculateRate_CalculatesRate()
        {
            // the deprivation is measured on a scale of 1-4 with a percentage in each bracket

            int oneDim = 86;
            int twoDim = 42;
            int threeDim = 11;
            int fourDim = 2;

            int total = 203;

            //act
            var result = _streetCheckerCalculator.CalculateRate(oneDim, twoDim, threeDim, fourDim);

            // assert
            result.Should().Be(9);
        }
    }
}