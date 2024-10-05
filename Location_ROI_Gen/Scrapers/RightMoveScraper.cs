using Location_ROI_Gen.Models;
using Location_ROI_Gen.Static;

namespace Location_ROI_Gen.Scrapers
{
    internal class RightMoveScraper : IRightMoveScraper
    {
        private readonly IAngleSharpWrapper _angleSharpWrapper;
        private List<int> listOfRadius = new List<int>() { 1, 3, 5 };
        public RightMoveScraper(IAngleSharpWrapper angleSharpWrapper)
        {
            _angleSharpWrapper = angleSharpWrapper;
        }
        public async Task<IList<House>> GetThreeBedPropertiesForSale(string key)
        {
            var houses = new List<House>();

            for (int i = 1; i < 3; i++)
            {
                //set price ascending to avoid skewing results
                string url = $"https://www.rightmove.co.uk/property-for-sale/find.html?searchType=SALE&locationIdentifier=REGION%{RightMoveCodes.CityToCode[key]}&insId=1&radius={listOfRadius[i]}.0&sortType=1&minBedrooms=3&maxBedrooms=3&displayPropertyType=&maxDaysSinceAdded=&_includeSSTC=on&sortByPriceAscending=&primaryDisplayPropertyType=&secondaryDisplayPropertyType=&oldDisplayPropertyType=&oldPrimaryDisplayPropertyType=&newHome=&auction=false";
                var document = await _angleSharpWrapper.GetSearchResults(url);
                if (!document.Title.ToLower().Contains(key.ToLower()))
                {
                    continue;
                }
                var searchResults = document.GetElementsByClassName("l-searchResults");
                var properties = searchResults[0].Children;
                if (properties.Any())
                {
                    houses.AddRange(properties.MapRM(key));
                }
            }

            return houses;
        }

        public async Task<IList<House>> GetThreeBedPropertiesToRent(string key)
        {
            var houses = new List<House>();
          
            var postCodeCounter = 0;
            for (int i = 0; i < 3; i++)
            {
                //sort type 1 is price ascending
                //region is inside london
                string url = $"https://www.rightmove.co.uk/property-to-rent/find.html?locationIdentifier=REGION%{RightMoveCodes.CityToCode[key]}&maxBedrooms=3&minBedrooms=3&radius={listOfRadius[i]}&sortType=1&propertyTypes=&includeLetAgreed=false&mustHave=&dontShow=&furnishTypes=&keywords=";
                var document = await _angleSharpWrapper.GetSearchResults(url);
                //if (document.Title!document.Title.ToLower().Contains(key.ToLower()))
                //{
                //    continue;
                //}
                var searchResults = document.GetElementsByClassName("l-searchResults");
                var properties = searchResults[0].Children;
                if (properties.Any())
                {
                    houses.AddRange(properties.MapRM(key));
                }
            }

            return houses;
        }
    }
}
