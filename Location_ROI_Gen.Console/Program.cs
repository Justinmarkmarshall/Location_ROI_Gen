
using Location_ROI_Gen.Calculator;
using Location_ROI_Gen.Dtos;
using Location_ROI_Gen.Scrapers;
using Location_ROI_Gen.Static;
using Location_ROI_Gen.Writers;
using Microsoft.Extensions.DependencyInjection;
using System.Formats.Asn1;



var serviceCollection = new ServiceCollection()
    .AddTransient<IAngleSharpWrapper, AngleSharpWrapper>()
    .AddTransient<ISpreadSheetWriter, SpreadSheetWriter>()
    .AddTransient<IStreetCheckerCalculator, StreetCheckerCalculator>()
    .AddTransient<IStreetCheckerScraper, StreetCheckerScraper>()
    .AddTransient<IRightMoveScraper, RightMoveScraper>()
    .AddLogging()
    .BuildServiceProvider();

var ssWriter = serviceCollection.GetService<ISpreadSheetWriter>();
var streetCheckerCalculator = serviceCollection.GetService<IStreetCheckerCalculator>();

var usingApp = true;

while (usingApp)
{
    Console.WriteLine("Would you like to search street checker for postcode data (1) or rightmove for property sold data? (2)");

    var response = Console.ReadLine();
    var searchingPostcodes = false;
    var searchingCities = false;


    if (response == "1")
    {
        Console.WriteLine("You have selected street checker, please enter the postcode you would like to search for.");
        searchingPostcodes = true;
    }
    else
    {
        Console.WriteLine("You have selected rightmove, please enter the city you would like to search for.");
        searchingCities = true;
    }


    while (searchingPostcodes)
    {
        Console.WriteLine("Hello, what postcode are you looking at today?");
        var postcode = Console.ReadLine();
        Console.WriteLine("Just a moment, I'm looking up the data for " + postcode);


        var streetCheckerScraper = serviceCollection.GetService<IStreetCheckerScraper>();

        var stat = await streetCheckerScraper.GetPostcodeStatistics(postcode);

        if (!stat.Contains("Error"))
        {
            ssWriter.WriteToSpreadSheet(stat);
        }
        else
        {
            Console.WriteLine("Something went wrong with that postcode, please try manually or debug.");
        }

        Console.WriteLine("Would you like to look up another postcode? (y/n)");
        var res = Console.ReadLine();

        if (res != "y")
        {
            searchingPostcodes = false;
            searchingCities = true;
        }
    }

    while (searchingCities)
    {
        Console.WriteLine("What city is that in?");
        var city = Console.ReadLine();
        Console.WriteLine("Just a moment, I'm looking up the data for " + city);

        var rightMoveScraper = serviceCollection.GetService<IRightMoveScraper>();
        var location = new Location() { Name = city };
        var oneBedRental = await rightMoveScraper.GetPropertiesToRentForCity(city, 1);

        location.Date = DateTime.Now;
        location.OneBedAverageRentPrice = RightMoveCalculator.CalculateAverage(oneBedRental.Select(r => r.Price).ToList(), oneBedRental.Count());

        var twoBedRental = await rightMoveScraper.GetPropertiesToRentForCity(city, 2);
        location.TwoBedAverageRentPrice = RightMoveCalculator.CalculateAverage(twoBedRental.Select(r => r.Price).ToList(), twoBedRental.Count());

        var threeBedRental = await rightMoveScraper.GetPropertiesToRentForCity(city, 3);
        location.ThreeBedAverageRentPrice = RightMoveCalculator.CalculateAverage(threeBedRental.Select(r => r.Price).ToList(), threeBedRental.Count());

        var twoBedSale = await rightMoveScraper.GetPropertiesForSaleForCity(city, 2);
        location.TwoBedAverageSalePrice = RightMoveCalculator.CalculateAverage(twoBedSale.Select(r => r.Price).ToList(), twoBedSale.Count());

        var threeBedSale = await rightMoveScraper.GetPropertiesForSaleForCity(city, 3);
        location.ThreeBedAverageSalePrice = RightMoveCalculator.CalculateAverage(threeBedSale.Select(r => r.Price).ToList(), threeBedSale.Count());

        location.TwoBedMortgage = MortgageCost.MortgageCostForHousePrice(location.TwoBedAverageSalePrice);
        location.ThreeBedMortgage = MortgageCost.MortgageCostForHousePrice(location.ThreeBedAverageSalePrice);

        var mortgageToRentDiffPc = (int)streetCheckerCalculator.CalculatePercentage(location.TwoBedAverageRentPrice, location.TwoBedMortgage);
        location.MortgageToRent_DiffPc = mortgageToRentDiffPc;

        location.MortgageToRent_DiffValue = location.ThreeBedAverageRentPrice - location.ThreeBedMortgage;

        ssWriter.WriteToCitySpreadsheet(location);

        Console.WriteLine("That's all done for you!");
        Console.WriteLine("thanks for using Location ROI Gen!");

        Console.WriteLine("Would you like to look up another city? (y/n)");
        var resp = Console.ReadLine();
        Console.WriteLine(resp);

        if (response != "y")
        {
            searchingCities = false;
        }
    }
}
