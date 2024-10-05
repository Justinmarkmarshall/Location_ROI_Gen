using Location_ROI_Gen.Calculator;
using Location_ROI_Gen.Data;
using Location_ROI_Gen.Dtos;
using Location_ROI_Gen.Scrapers;
using Location_ROI_Gen.Static;
using Location_ROI_Gen.Writers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace Location_ROI_Gen
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
               .Build();

            var serviceCollection = new ServiceCollection()
                .AddDbContext<DataContext>(options =>
                {
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                    options.EnableDetailedErrors(true);
                })
                .AddScoped<DbContext, DataContext>()
                .AddTransient<IAngleSharpWrapper, AngleSharpWrapper>()
                .AddTransient<IRightMoveScraper, RightMoveScraper>()
                .AddTransient<IEFWrapper, EFWrapper>()
                .AddTransient<IStreetCheckerScraper, StreetCheckerScraper>()
                .AddTransient<ISpreadSheetWriter, SpreadSheetWriter>()
                .AddTransient<IStreetCheckerCalculator, StreetCheckerCalculator>()
                .AddTransient<ILinkedInScraper, LinkedInScraper>()
                .AddLogging()
                .BuildServiceProvider();

            var rmScraper = serviceCollection.GetService<IRightMoveScraper>();
            var efWrapper = serviceCollection.GetService<IEFWrapper>();
            var ssWriter = serviceCollection.GetService<ISpreadSheetWriter>();

            #region City data generator
            List<Location> locations = new();

            var key = "Lancaster";
                var rmSaleResults = await rmScraper.GetPropertiesForSaleForCity(key);
                var rmRentResults = await rmScraper.GetPropertiesToRentForCity(key);

                if (rmSaleResults.Count > 0 && rmRentResults.Count > 0)
                {
                    var halfWay = (double)(rmSaleResults.Count / 2);

                    var medianSalePrice = rmSaleResults.OrderByDescending(r => r.Price).ToList()[(int)halfWay].Price;

                    var averageSalePrice = RightMoveCalculator.CalculateAverage(rmSaleResults.Select(r => r.Price).ToList(), rmSaleResults.Count());
                    var averageRentPrice = RightMoveCalculator.CalculateAverage(rmRentResults.Select(r => r.Price).ToList(), rmRentResults.Count());

                    var location = new Location() { Name = key, Date = DateTime.UtcNow };
                    location.ThreeBedAverageSalePrice = averageSalePrice;
                    location.ThreeBedHouseAverageRentPrice = averageRentPrice;

                    var mortgageCost = MortgageCost.MortgageCostForHousePrice(averageSalePrice);
                    location.AveragePriceMortgage = mortgageCost;

                    var mortgageToRentDiffPc = RightMoveCalculator.CalculateMortgageToRentDiffPc(mortgageCost, averageRentPrice);
                    location.MortgageToRent_DiffPc = mortgageToRentDiffPc;

                    location.MortgageToRent_DiffValue = averageRentPrice - mortgageCost;

                    locations.Add(location);
                }
            

            //await efWrapper.UpsertLocations(locations);

            //Console.WriteLine($"gathered infor for {locations.Count} locations");

            //foreach (var location in locations)
            //    Console.WriteLine($"{location.Name} has a 3 bed average sale price of {location.ThreeBedAverageSalePrice}, a 3 bed average rental price of {location.ThreeBedHouseAverageRentPrice}, " +
            //        $"a 3 bed average mortgage price of {location.AveragePriceMortgage} and a Mortgage to rent Difference of {location.MortgageToRent_DiffPc}% and a Mortgage to rent Difference value of {location.MortgageToRent_DiffValue}");
            #endregion City data generator

            #region postcode data generator

            //var streetChecker = serviceCollection.GetService<IStreetCheckerScraper>();
            //var stat = await streetChecker.GetPostcodeStatistics("ls121sl");
            //ssWriter.WriteToSpreadSheet(stat);


            #endregion postcode data generator

            #region linked in jobs generator

            var linkedInScraper = serviceCollection.GetService<ILinkedInScraper>();
            var result = linkedInScraper.GetDotNetDevPostingsForCity("Leeds");

            #endregion linked in jobs generator

            #region rightMovePropertyData


            #endregion rightMovePropertyData
            //var rmSaleResults = await rmScraper.GetThreeBedPropertiesForSale("Le
            //process properties by town
        }
    }
}