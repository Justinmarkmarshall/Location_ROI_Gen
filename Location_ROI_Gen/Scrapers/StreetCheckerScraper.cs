using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Location_ROI_Gen.Calculator;
using System.Text;

namespace Location_ROI_Gen.Scrapers
{
    public class StreetCheckerScraper : IStreetCheckerScraper
    {
        private readonly IAngleSharpWrapper _angleSharpWrapper;
        private readonly IStreetCheckerCalculator _streetCheckerCalculator;

        public StreetCheckerScraper(IAngleSharpWrapper angleSharpWrapper, IStreetCheckerCalculator streetCheckerCalculator)
        {
            _angleSharpWrapper = angleSharpWrapper;
            _streetCheckerCalculator = streetCheckerCalculator;
        }

        public async Task<string> GetPostcodeStatistics(string postcode)
        {
            try
            {
                string url = $"https://www.streetcheck.co.uk/postcode/{postcode}";
                var document = await _angleSharpWrapper.GetSearchResults(url);

                StringBuilder result = new StringBuilder();
                result.Append(postcode + ",");

                var infoPieces = document.GetElementsByClassName("info-piece");
                result.Append(GetHousing(infoPieces));
                result.Append(GetHealth(infoPieces));
                result.Append(GetEducation(infoPieces));
                result.Append(GetEthnicGroups(infoPieces));
                result.Append(GetReligion(infoPieces));
                result.Append(GetEmployment(infoPieces));
                result.Append(GetSocioEconomic(infoPieces));
                result.Append(Environment.NewLine);

                return result.ToString();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        private string GetHousing(IHtmlCollection<IElement> infoPieces)
        {
            var housingTenure = infoPieces.Where(r => r.TextContent.Contains("Housing Tenure")).FirstOrDefault();
            var innerHtml = housingTenure.GetElementsByClassName("table table-striped table-hover");
            var result = new StringBuilder();
            //instead try first and get the parent element
            //var chartAblePie = occupancy.GetElementsByClassName("row chartable-pie");

            var rows = innerHtml.First() as IHtmlTableElement;

            var cells = rows.Bodies.First().Children;
            // housing tenure
            var ownedOutright = cells.First().Children.Skip(1).First().TextContent;
            var ownedWithMortgage = cells.Skip(1).First().Children.Skip(1).First().TextContent;
            var sharedOwnership = cells.Skip(2).First().Children.Skip(1).First().TextContent;
            var rentedFromCouncil = cells.Skip(3).First().Children.Skip(1).First().TextContent.Trim();
            var rentedFromSocial = cells.Skip(4).First().Children.Skip(1).First().TextContent.Trim();
            var rentedFromPrivateLandlord = cells.Skip(5).First().Children.Skip(1).First().TextContent.Trim();
            var rentedFromOther = cells.Skip(6).First().Children.Skip(1).First().TextContent.Trim();
            var rentFree = cells.Skip(7).First().Children.Skip(1).First().TextContent.Trim();

            var householdComposition = infoPieces.Where(r => r.TextContent.Contains("Household Composition")).FirstOrDefault();

            var householdCompositionTable = householdComposition.GetElementsByClassName("table table-striped table-hover");
            var householdCompositionRows = householdCompositionTable.First() as IHtmlTableElement;
            var compositionCells = householdCompositionRows.Bodies.First().Children;

            var familyHouseHold = compositionCells.Skip(1).First().Children.Skip(1).First().TextContent;

            var deprivation = infoPieces.Where(r => r.TextContent.Contains("Household Deprivation")).FirstOrDefault();
            var depTable = deprivation.GetElementsByClassName("table table-striped table-hover");
            var depRows = depTable.First() as IHtmlTableElement;
            var depCells = depRows.Bodies.First().Children;

            var notDeprived = depCells.First().Children.Skip(1).First().TextContent;
            var oneDimension = depCells.Skip(1).First().Children.Skip(1).First().TextContent;
            var twoDimension = depCells.Skip(2).First().Children.Skip(1).First().TextContent;
            var threeDimension = depCells.Skip(3).First().Children.Skip(1).First().TextContent.Trim();
            var fourDimension = depCells.Skip(4).First().Children.Skip(1).First().TextContent.Trim();

            var totalRow = depTable.Last() as IHtmlTableElement;
            var total = totalRow.Children.Last().Children.First().Children.Last().TextContent.Trim();

            //calculate the ownedOutright percentage of total
            var ownedOutrightPercentage = _streetCheckerCalculator.CalculatePercentage(int.Parse(ownedOutright), int.Parse(total));
            result.Append(ownedOutrightPercentage + ",");

            //calculate the private rented percentage of total
            var privateRentedPercentage = _streetCheckerCalculator.CalculatePercentage(int.Parse(rentedFromPrivateLandlord), int.Parse(total));
            result.Append(privateRentedPercentage + ",");

            //calculate the council rented percentage of total
            var councilRentedPercentage = _streetCheckerCalculator.CalculatePercentage(int.Parse(rentedFromCouncil) + int.Parse(rentedFromSocial), int.Parse(total));
            result.Append(councilRentedPercentage + ",");

            //calculate the family households percentage of total
            var familyHouseholdsPercentage = _streetCheckerCalculator.CalculatePercentage(int.Parse(familyHouseHold), int.Parse(total));
            result.Append(familyHouseholdsPercentage + ",");

            //calculate the not deprived percentage of total
            var notDeprivedPercentage = _streetCheckerCalculator.CalculatePercentage(int.Parse(notDeprived), int.Parse(total));
            result.Append(notDeprivedPercentage + ",");

            //calculate the rate of deprivation
            var rateOfDeprivation = _streetCheckerCalculator.CalculateRate(int.Parse(oneDimension), int.Parse(twoDimension), int.Parse(threeDimension), int.Parse(fourDimension));
            result.Append(rateOfDeprivation + ",");

            return result.ToString();

        }
    
        private string GetHealth(IHtmlCollection<IElement> infoPieces)
        {
            var health = infoPieces.Where(r => r.TextContent.Contains("Health")).FirstOrDefault();
            var healthTable = health.GetElementsByClassName("table table-striped table-hover");
            var result = new StringBuilder();
            //instead try first and get the parent element

            var rows = healthTable.First() as IHtmlTableElement;

            var cells = rows.Bodies.First().Children;
            var veryGood = cells.First().Children.Skip(1).First().TextContent;
            var good = cells.Skip(1).First().Children.Skip(1).First().TextContent;

            var totalRow = healthTable.Last() as IHtmlTableElement;
            var total = totalRow.Children.Last().Children.First().Children.Last().TextContent.Trim();
            result.Append(_streetCheckerCalculator.CalculatePercentage((int.Parse(veryGood) + int.Parse(good)), int.Parse(total)) + ",");
            return result.ToString();
        }

        private string GetEducation(IHtmlCollection<IElement> infoPieces)
        {
            var education = infoPieces.Where(r => r.TextContent.Contains("Education")).FirstOrDefault();
            var educationTable = education.GetElementsByClassName("table table-striped table-hover");
            var result = new StringBuilder();
            var rows = educationTable.First() as IHtmlTableElement;

            var cells = rows.Bodies.First().Children;
            var degree = cells.First().Children.Skip(1).First().TextContent;
            var apprenticeship = cells.Skip(1).First().Children.Skip(1).First().TextContent;
            var aLevels = cells.Skip(2).First().Children.Skip(1).First().TextContent;
            var noGcses = cells.Skip(5).First().Children.Skip(1).First().TextContent;


            var totalRow = educationTable.Last() as IHtmlTableElement;
            var total = totalRow.Children.Last().Children.First().Children.Last().TextContent.Trim();
            result.Append(_streetCheckerCalculator.CalculatePercentage((int.Parse(degree) + int.Parse(apprenticeship)), int.Parse(total)) + ",");
            result.Append(_streetCheckerCalculator.CalculatePercentage(int.Parse(noGcses), int.Parse(total)) + ",");

            return result.ToString();
        }

        private string GetEthnicGroups(IHtmlCollection<IElement> infoPieces)
        {
            //this is failing to find the info pieces for ethnic group
            var ethnicGroups = infoPieces.Where(r => r.TextContent.Contains("Ethnic Group")).FirstOrDefault();
            var result = new StringBuilder();
            var ethnicGroupTable = ethnicGroups.GetElementsByClassName("table table-striped table-hover");
            var rows = ethnicGroupTable.First() as IHtmlTableElement;

            var cells = rows.Bodies.First().Children;
            var whiteBritish = cells.First().Children.Skip(1).First().TextContent;

            var totalRow = ethnicGroupTable.Last() as IHtmlTableElement;
            var total = totalRow.Children.Last().Children.First().Children.Last().TextContent.Trim();
            result.Append(_streetCheckerCalculator.CalculatePercentage(int.Parse(whiteBritish), int.Parse(total)) + ",");
            return result.ToString();
        }

        private string GetReligion(IHtmlCollection<IElement> infoPieces)
        {
            var religion = infoPieces.Where(r => r.OuterHtml.Contains("Religion")).FirstOrDefault();
            var result = new StringBuilder();
            var religionTable = religion.GetElementsByClassName("table table-striped table-hover");
            var rows = religionTable.First() as IHtmlTableElement;

            var cells = rows.Bodies.First().Children;
            var christian = rows.Bodies.First().Children[1].Children[1].TextContent;
            var total = rows.Rows[10].Children[1].TextContent;
            result.Append(_streetCheckerCalculator.CalculatePercentage(int.Parse(christian), int.Parse(total)) + ",");
            return result.ToString();
        }

        private string GetEmployment(IHtmlCollection<IElement> infoPieces)
        {
            var employment = infoPieces.Where(r => r.TextContent.Contains("Economic Activity")).FirstOrDefault();
            var result = new StringBuilder();
            var employmentTable = employment.GetElementsByClassName("table table-striped table-hover");
            var rows = employmentTable.First() as IHtmlTableElement;

            var cells = rows.Bodies.First().Children;
            var employed = cells.First().Children.Skip(1).First().TextContent;
            var ptEmployed = cells.Skip(1).First().Children.Skip(1).First().TextContent;
            var selfEmployed = cells.Skip(2).First().Children.Skip(1).First().TextContent;
            var unemployed = cells.Skip(4).First().Children.Skip(1).First().TextContent;
            var student = cells.Skip(5).First().Children.Skip(1).First().TextContent;
            var retired = cells.Skip(6).First().Children.Skip(1).First().TextContent;
            var lookingAfterHome = cells.Skip(7).First().Children.Skip(1).First().TextContent;
            var longTermSick = cells.Skip(8).First().Children.Skip(1).First().TextContent;

            var totalRow = employmentTable.Last() as IHtmlTableElement;
            var total = totalRow.Children.Last().Children.First().Children.Last().TextContent.Trim();
            result.Append(_streetCheckerCalculator.CalculatePercentage((int.Parse(employed) + int.Parse(ptEmployed) + int.Parse(selfEmployed)), int.Parse(total)) + ",");
            result.Append(_streetCheckerCalculator.CalculatePercentage(int.Parse(unemployed) + int.Parse(lookingAfterHome) + int.Parse(longTermSick), int.Parse(total)) + ",");
            result.Append(_streetCheckerCalculator.CalculatePercentage(int.Parse(retired), int.Parse(total)) + ",");

            return result.ToString();
        }

        private string GetSocioEconomic(IHtmlCollection<IElement> infoPieces)
        {
            var socioEconomic = infoPieces.Where(r => r.TextContent.Contains("Socio-Economic"));
            var result = new StringBuilder();
            var socioEconomicTable = socioEconomic.First().GetElementsByClassName("table table-striped table-hover");
            var rows = socioEconomicTable.First() as IHtmlTableElement;

            var cells = rows.Bodies.First().Children;
            var high = int.Parse(cells.First().Children.Skip(1).First().TextContent) + int.Parse(cells.Skip(2).First().Children.Skip(1).First().TextContent) + int.Parse(cells.Skip(3).First().Children.Skip(1).First().TextContent);
            // assign medium as the sum of the next three cells
            var lowTech = int.Parse(cells.Skip(4).First().Children.Skip(1).First().TextContent);
            var routine = int.Parse(cells.Skip(5).First().Children.Skip(1).First().TextContent);
            var semiRoutine = int.Parse(cells.Skip(6).First().Children.Skip(1).First().TextContent);
            var mid = lowTech + routine + semiRoutine;

            var longTermUnemployed = int.Parse(cells.Skip(7).First().Children.Skip(1).First().TextContent);
            var low = longTermUnemployed;

            var student = int.Parse(cells.Skip(8).First().Children.Skip(1).First().TextContent);

            var totalRow = socioEconomicTable.Last() as IHtmlTableElement;
            var total = totalRow.Children.Last().Children.First().Children.Last().TextContent.Trim();
            result.Append(_streetCheckerCalculator.CalculatePercentage(high, int.Parse(total)) + ",");
            result.Append(_streetCheckerCalculator.CalculatePercentage(mid, int.Parse(total)) + ",");
            result.Append(_streetCheckerCalculator.CalculatePercentage(low, int.Parse(total)) + ",");
            result.Append(_streetCheckerCalculator.CalculatePercentage(student, int.Parse(total)) + ",");

            return result.ToString();
            
        }
    }

    public interface IStreetCheckerScraper
    {
        public Task<string> GetPostcodeStatistics(string postcode);
    }
}
