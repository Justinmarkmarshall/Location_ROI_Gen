namespace Location_ROI_Gen.Calculator
{
    public class StreetCheckerCalculator : IStreetCheckerCalculator
    {
        public double CalculatePercentage(int number, int total)
        {
            var divide = (double)number / (double)total;

            return Math.Round(divide * 100);
        }

        public double CalculateRate(int oneDim, int twoDim, int threeDim, int fourDim)
        {
            var totalDim = oneDim + twoDim + threeDim + fourDim;

            var threeDimPercentage = CalculatePercentage(threeDim, totalDim);
            var fourDimPercentage = CalculatePercentage(fourDim, totalDim);
            var value = threeDimPercentage + fourDimPercentage;

            return Math.Round(value);
        }
    }

    public interface IStreetCheckerCalculator
    {
        public double CalculatePercentage(int number, int total);
        public double CalculateRate(int oneDim, int twoDim, int threeDim, int fourDim);
    }
}
