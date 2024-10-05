using System.ComponentModel.DataAnnotations;

namespace Location_ROI_Gen.Dtos
{
    public class Location
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public int ThreeBedAverageSalePrice { get; set; } = 0;
        public int ThreeBedHouseAverageRentPrice { get; set; } = 0;
        public int AveragePriceMortgage { get; set; }
        public int MortgageToRent_DiffPc { get; set; }
        public int MortgageToRent_DiffValue { get; set; }
        public DateTime Date { get; set; } 
    }
}
