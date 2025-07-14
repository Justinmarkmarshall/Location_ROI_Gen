using Location_ROI_Gen.Models;

namespace Location_ROI_Gen.Scrapers
{
    public interface IRightMoveScraper
    {
        /// <summary>
        /// rooms will be min and max to avoid duplicate data
        /// </summary>
        /// <param name="key"></param>
        /// <param name="rooms"></param>
        /// <returns></returns>
        public Task<IList<House>> GetPropertiesForSaleForCity(string city, int rooms = 3);

        public Task<IList<House>> GetPropertiesToRentForCity(string city, int rooms = 3);
    }
}