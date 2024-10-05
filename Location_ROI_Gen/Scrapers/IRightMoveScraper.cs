using Location_ROI_Gen.Models;

namespace Location_ROI_Gen.Scrapers
{
    internal interface IRightMoveScraper
    {
        Task<IList<House>> GetThreeBedPropertiesForSale(string key);

        Task<IList<House>> GetThreeBedPropertiesToRent(string key);
    }
}