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

        /// <summary>
        /// takes in a comma separated list of aggregated results and writes them to a csv file
        /// </summary>
        /// <param name="aggregatedResults"></param>
        public void WriteToSpreadSheet(string aggregatedResults)
        {
            string spreadsheetName = "Location_ROI_Gen.csv";
            //string path = $"C:\\Users\\{Environment.UserName}\\Desktop\\{spreadsheetName}";
            string test = $"C:\\Users\\justi\\OneDrive\\Personal\\Work_Professional_Financial\\Documents\\Location_ROI_Gen\\{spreadsheetName}";

            //create a current path which is in the current directory
            var directory = Directory.GetCurrentDirectory();
            string path = $"{Directory.GetCurrentDirectory()}\\..\\..\\..\\..\\Location_ROI_Gen\\Spreadsheets\\{spreadsheetName}";

            //C:\Dev\Location_ROI_Generator\Location_ROI_Gen\Location_ROI_Gen\Spreadsheets\
            if (!File.Exists(path)){
                File.Create(path).Close();
                using StreamWriter sw = File.AppendText(path);
                sw.WriteLine(summaryHeaders);
                sw.WriteLine(aggregatedResults);
                sw.Close();
                File.OpenRead(path);
            }
            else{                
                //File.Open(path, FileMode.Append);
                using StreamWriter sw = File.AppendText(path);
                sw.WriteLine(aggregatedResults);
                sw.Close();
                File.OpenRead(path);
            }
            

            

        }       
    }

    public interface ISpreadSheetWriter
    {
        public void WriteToSpreadSheet(string aggregatedResults);
    }
}
