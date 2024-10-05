using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location_ROI_Gen.Static
{
    public class FormatPostcode
    {
        /// <summary>
        /// this is very difficult, and I cannot find the RM documentation onn it.
        /// </summary>
        /// <param name="postcode"></param>
        /// <returns></returns>
        public string MapToFormattedPostcode(string postcode)
        {
            StringBuilder result = new StringBuilder();
            //LOOP through the postcode
            for (int i = 0; i < postcode.Length; i++)
            {
                //If the character is a digit
                if (char.IsDigit(postcode[i]) && char.IsLetter(postcode[i-1]))
                {
                    result.Append($"/{postcode[i]}");                    
                }
                else if (char.IsDigit(postcode[i]))
                {
                    result.Append(postcode[i]);
                }
                else if (i > 3 && char.IsLetter(postcode[i]) && char.IsLetter(postcode[i - 1])){
                    result.Append($"/{postcode[i]}");
                }
                else if (char.IsLetter(postcode[i]))
                {
                    result.Append(postcode[i]);
                }
            }

            return result.ToString().ToUpper();
        }
    }   
}
