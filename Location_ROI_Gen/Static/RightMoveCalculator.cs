namespace Location_ROI_Gen.Static
{
    public static class RightMoveCalculator
    {
        public static int CalculateAverage(List<int> prices, int numberOfPrices)
        {
            if (prices.Count == 0) return 0;
            return prices.Sum() / numberOfPrices;
        }

        public static int CalculateMortgageToRentDiffPc(int mortgageCost, int averageRentPrice)
        {
            var diff = averageRentPrice - mortgageCost;

            if (diff > 0)
            {
                //calulate diff as a percentage of the mortgage cost
                return mortgageCost / diff;
            }
            else return -1;
        }
    }
}
