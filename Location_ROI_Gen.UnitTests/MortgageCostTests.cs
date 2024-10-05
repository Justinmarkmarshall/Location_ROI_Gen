using Location_ROI_Gen.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location_ROI_Gen.UnitTests
{
    public class MortgageCostTests
    {
        public MortgageCostTests()
        {

        }

        [Theory]
        [InlineData(80000, 316)]
        [InlineData(85432, 344)]
        [InlineData(100000, 422)]
        [InlineData(120000, 528)]
        [InlineData(140000, 633)]
        [InlineData(160000, 739)]
        [InlineData(180000, 844)]
        public void MortgageCost_CalculatesMortgageCost(int housePrice, int expected)
        {

            // Arrange

            // Act
            int result = MortgageCost.MortgageCostForHousePrice(housePrice);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}

