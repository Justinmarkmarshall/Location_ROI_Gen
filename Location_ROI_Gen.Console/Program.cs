
using Location_ROI_Gen.Calculator;
using Location_ROI_Gen.Scrapers;
using Location_ROI_Gen.Writers;
using Microsoft.Extensions.DependencyInjection;



var serviceCollection = new ServiceCollection()
    .AddTransient<IAngleSharpWrapper, AngleSharpWrapper>()
    .AddTransient<ISpreadSheetWriter, SpreadSheetWriter>()
    .AddTransient<IStreetCheckerCalculator, StreetCheckerCalculator>()
    .AddTransient<IStreetCheckerScraper, StreetCheckerScraper>()
    .AddLogging()
    .BuildServiceProvider();

var usingTheApp = true;

while (usingTheApp)
{

    Console.WriteLine("Hello, what postcode are you looking at today?");
    var postcode = Console.ReadLine();
    Console.WriteLine("Just a moment, I'm looking up the data for " + postcode);

    var ssWriter = serviceCollection.GetService<ISpreadSheetWriter>();
    var streetCheckerCalculator = serviceCollection.GetService<IStreetCheckerCalculator>();
    var streetCheckerScraper = serviceCollection.GetService<IStreetCheckerScraper>();

    var stat = await streetCheckerScraper.GetPostcodeStatistics(postcode);

    if (!stat.Contains("Error"))
    {
        ssWriter.WriteToSpreadSheet(stat);
        Console.WriteLine("That's all done for you!");
        Console.WriteLine("thanks for using Location ROI Gen!");
    }
    else
    {
        Console.WriteLine("Something went wrong with that postcode, please try manually or debug.");
    }

}
