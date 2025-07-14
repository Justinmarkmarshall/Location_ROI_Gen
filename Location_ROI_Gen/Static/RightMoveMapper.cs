using AngleSharp.Dom;
using Location_ROI_Gen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location_ROI_Gen.Static
{
    public static class RightMoveMapper
    {
        public static List<House> MapModernisedRM(this IHtmlCollection<IElement> searchResult, string location)
        {
            var houses = new List<House>();

            searchResult.Count();

            for (int i = 0; i < searchResult.Count(); i++)
            {
                try
                {
                    IElement property = searchResult[i].Children[0];
                    if (property != null && property.InnerHtml != null)
                    {
                        var allINeed = property.InnerHtml;
                        var address = property.GetElementsByClassName("propertyCard-details")[0].
                        GetElementsByClassName("propertyCard-address")[0].GetElementsByTagName("span")[0].InnerHtml;
                        var pce = property.GetElementsByClassName("propertyCard-priceValue")[0].InnerHtml;
                        var aTags = property.QuerySelector("a").Id;
                        var sanitizedPrice = Convert.ToInt32(pce.Replace("£", "").Replace(",", "").Replace("pcm", "").Trim());
                        if (!allINeed.ToLower().Contains("hotel")
                            && !allINeed.ToLower().Contains("retirement")
                            && !allINeed.ToLower().Contains("investment only")
                            && !allINeed.ToLower().Contains("cash buyers only")
                            && !allINeed.ToLower().Contains("shared ownership")
                            && !allINeed.ToLower().Contains("55")
                            && !allINeed.ToLower().Contains("share")
                            && !allINeed.ToLower().Contains("in need of modernisation")) 
                            houses.Add(new House()
                            {
                                Price = sanitizedPrice,
                                Link = $"https://www.rightmove.co.uk/properties/{aTags.Replace("prop", "")}#/?channel=RES_BUY",
                                location = location
                            });
                    }
                }
                catch (Exception ex)
                {
                    continue;
                }
            }

            #region deprecatedCode
            //foreach (var property in searchResult[0].Children)
            //{
            //    var allINeed = property.InnerHtml;

            //    var address = property.GetElementsByClassName("propertyCard-details")[0].
            //        GetElementsByClassName("propertyCard-address")[0].GetElementsByTagName("span")[0].InnerHtml;
            //    var pce = property.GetElementsByClassName("propertyCard-priceValue")[0].InnerHtml;
            //    var aTags = property.QuerySelector("a").Id;
            //    if (!allINeed.ToLower().Contains("hotel")
            //        && !allINeed.ToLower().Contains("retirement")
            //        && !allINeed.ToLower().Contains("investment only")
            //        && !allINeed.ToLower().Contains("cash buyers only")
            //        && !allINeed.ToLower().Contains("shared ownership")
            //        && !allINeed.ToLower().Contains("share")) houses.Add(new House()
            //        {
            //            Area = address.ToString(),
            //            Link = $"https://www.rightmove.co.uk/properties/{aTags.Replace("prop", "")}#/?channel=RES_BUY",
            //            Price = pce
            //        });                
            //}
            #endregion DeprecatedCode
            return houses;
        }
    }
}
