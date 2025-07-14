using Location_ROI_Gen.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location_ROI_Gen.Writers
{
    public class SpreadSheetWriter : ISpreadSheetWriter
    {
        private const string summaryHeaders = "Postcode,Owned,PrivateRented,CouncilRented,FamilyHouseholds,NotDeprived,RateOfDeprivation,Health,EducationHigh,EducationLow,White,Christian,Employed,Unemployed,Retired,SocioEconomic_High,SocioEconomic_Mid,SocioEconomic_Low,Student";

        private const string cityHeaders = "City,Date,1Bed,2BedSale,Mortgage,Rent,Diff,DiffPc,3BedSale,Mortgage,Rent";
        /// <summary>
        /// takes in a comma separated list of aggregated results and writes them to a csv file
        /// </summary>
        /// <param name="aggregatedResults"></param>
        public void WriteToSpreadSheet(string aggregatedResults)
        {
            string spreadsheetName = "Location_ROI_Gen.csv";
            string path = $"{Directory.GetCurrentDirectory()}\\..\\..\\..\\..\\Location_ROI_Gen\\Spreadsheets\\{spreadsheetName}";

            //C:\Dev\Location_ROI_Generator\Location_ROI_Gen\Location_ROI_Gen\Spreadsheets\
            if (!File.Exists(path)) {
                File.Create(path).Close();
                using StreamWriter sw = File.AppendText(path);
                sw.WriteLine(summaryHeaders);
                sw.WriteLine(aggregatedResults);
                sw.Close();
                File.OpenRead(path);
            }
            else {
                //File.Open(path, FileMode.Append);
                using StreamWriter sw = File.AppendText(path);
                sw.WriteLine(aggregatedResults);
                sw.Close();
                File.OpenRead(path);
            }
        }

        public void WriteToCitySpreadsheet(Location location)
        {
            string spreadsheetName = "Location_ROI_Gen_City.csv";
            string path = $"{Directory.GetCurrentDirectory()}\\..\\..\\..\\..\\Location_ROI_Gen\\Spreadsheets\\{spreadsheetName}";
            var cityResults = $"{location.Name},{location.Date},£{location.OneBedAverageRentPrice} pcm,£{location.TwoBedAverageSalePrice},£{location.TwoBedMortgage}," +
                $"£{location.TwoBedAverageRentPrice} pcm,£{location.MortgageToRent_DiffValue} pcm,%{location.MortgageToRent_DiffPc},£{location.ThreeBedAverageSalePrice},£{location.ThreeBedMortgage},£{location.ThreeBedAverageRentPrice} pcm";

            //C:\Dev\Location_ROI_Generator\Location_ROI_Gen\Location_ROI_Gen\Spreadsheets\
            if (!File.Exists(path))
            {
                File.Create(path).Close();
                using StreamWriter sw = File.AppendText(path);
                sw.WriteLine(cityHeaders);
                sw.WriteLine(cityResults);
                sw.Close();
                File.OpenRead(path);
            }
            else
            {
                //File.Open(path, FileMode.Append);
                using StreamWriter sw = File.AppendText(path);
                sw.WriteLine(cityResults);
                sw.Close();
                File.OpenRead(path);
            }
        }
    }

    public interface ISpreadSheetWriter
    {
        public void WriteToSpreadSheet(string aggregatedResults);

        public void WriteToCitySpreadsheet(Location location);
    }
}
