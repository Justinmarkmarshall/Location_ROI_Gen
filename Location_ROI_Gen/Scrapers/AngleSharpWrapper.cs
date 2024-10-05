using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Io;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location_ROI_Gen.Scrapers
{
    public class AngleSharpWrapper : IAngleSharpWrapper
    {
        // this is too specific, it needs to return the document to the ZooplaScraper, where he can get by 
        // classname
        // this means I can query different
        public async Task<IDocument> GetSearchResults(string url, IRequester? requester = null)
        {
            if (requester == null)
            {
                requester = new DefaultHttpRequester();
            }

            var config = Configuration.Default.WithDefaultLoader().With(requester);
            var context = BrowsingContext.New(config);

            return await context.OpenAsync(url);
        }

        public async Task<IDocument> OpenAsync(string url)
        {
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);

            return await context.OpenAsync(url);

            //return document.GetElementsByClassName("css-1anhqz4-ListingsContainer earci3d2");
        }
    }

    public interface IAngleSharpWrapper
    {
        Task<IDocument> GetSearchResults(string url, IRequester? requester = null);
        Task<IDocument> OpenAsync(string url);
    }
}
