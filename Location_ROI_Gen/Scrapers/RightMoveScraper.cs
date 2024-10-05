using Location_ROI_Gen.Models;
using Location_ROI_Gen.Static;

namespace Location_ROI_Gen.Scrapers
{
    public class RightMoveScraper : IRightMoveScraper
    {
        private readonly IAngleSharpWrapper _angleSharpWrapper;
        private List<int> listOfRadius = new List<int>() { 1, 3, 5 };

        public RightMoveScraper(IAngleSharpWrapper angleSharpWrapper)
        {
            _angleSharpWrapper = angleSharpWrapper;
        }
        public async Task<IList<House>> GetPropertiesForSaleForCity(string city, int rooms)
        {
            var houses = new List<House>();

            var rmCode = RightMoveCodes.CityToCode[city];

            for (int i = 1; i < 3; i++)
            {
                //set price ascending to avoid skewing results
                string url = $"https://www.rightmove.co.uk/property-for-sale/find.html?searchType=SALE&locationIdentifier=REGION%{rmCode}&insId=1&radius={listOfRadius[i]}.0&sortType=1&minBedrooms=3&maxBedrooms=3&displayPropertyType=&maxDaysSinceAdded=&_includeSSTC=on&sortByPriceAscending=&primaryDisplayPropertyType=&secondaryDisplayPropertyType=&oldDisplayPropertyType=&oldPrimaryDisplayPropertyType=&newHome=&auction=false";
                var document = await _angleSharpWrapper.GetSearchResults(url);
                if (!document.Title.ToLower().Contains(city.ToLower()))
                {
                    continue;
                }
                var searchResults = document.GetElementsByClassName("l-searchResults");
                var properties = searchResults[0].Children;
                if (properties.Any())
                {
                    houses.AddRange(properties.MapModernisedRM(city));
                }
            }

            return houses;
        }

        public async Task<IList<House>> GetPropertiesToRentForCity(string city, int rooms = 3)
        {
            var houses = new List<House>();
            var iteration = 0;
            try
            {
                string rmCode = RightMoveCodes.CityToCode[city];

                var postCodeCounter = 0;
                for (int i = 0; i < 3; i++)
                {
                    iteration = i;
                    //sort type 1 is price ascending
                    //region is inside london
                    string url = $"https://www.rightmove.co.uk/property-to-rent/find.html?locationIdentifier=REGION%{rmCode}&maxBedrooms={rooms}&minBedrooms={rooms}&radius={listOfRadius[i]}&sortType=1&propertyTypes=&includeLetAgreed=false&mustHave=&dontShow=&furnishTypes=&keywords=";
                    var document = await _angleSharpWrapper.GetSearchResults(url);
                    //if (document.Title!document.Title.ToLower().Contains(key.ToLower()))
                    //{
                    //    continue;
                    //}
                    var searchResults = document.GetElementsByClassName("l-searchResults");
                    var properties = searchResults[0].Children;
                    if (properties.Any())
                    {
                        houses.AddRange(properties.MapModernisedRM(city));
                    }
                }

                return houses;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Something went wrong with the RightMoveScraper for {city} and {rooms} with radius {listOfRadius[iteration]}");
                return houses;
            }
        }
    }
}