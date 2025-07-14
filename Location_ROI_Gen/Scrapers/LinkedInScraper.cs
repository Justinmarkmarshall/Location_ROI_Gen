using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location_ROI_Gen.Scrapers
{
    /// <summary>
    /// it seems that LinkedIn have geoIds for each location, potentially by postcode
    /// </summary>
    public class LinkedInScraper : ILinkedInScraper
    {
        private readonly IAngleSharpWrapper _angleSharpWrapper;

        public Dictionary<string, string> geoIds = new Dictionary<string, string>()
        {
            { "derby", "103965606" },
            { "leeds", "102943586" }
        };

        public LinkedInScraper(IAngleSharpWrapper angleSharpWrapper)
        {
            _angleSharpWrapper = angleSharpWrapper;
        }

        public int GetDotNetDevPostingsForCity(string city)
        {
            string url = $"https://www.linkedin.com/jobs/search/?geoId={geoIds[city.ToLower()]}&keywords=.net%20developer";

            var document = _angleSharpWrapper.GetSearchResults(url).Result;
            var results = document.GetElementsByClassName("jobs-search-results-list__subtitle");

            //var test = document.GetElementsByClassName("display-flex t-normal t-12 t-black--light jobs-search-results-list__text");

            //var man = document.GetElementById("main");
            //var number = document.Title.Split(" ")[0];
            //var result = Convert.ToInt32(number);

            var tesla = document.GetElementsByTagName("body");

            return 0;
        }

        public string GetLinkedInData(string key)
        {
            throw new NotImplementedException();
        }
    }

    public interface ILinkedInScraper
    {
        public int GetDotNetDevPostingsForCity(string city);
    }
}
