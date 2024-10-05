namespace Location_ROI_Gen.Static
{
    public static class Calculator
    {
        public static int CalculateAverage(List<int> prices, int numberOfPrices)
        {
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
