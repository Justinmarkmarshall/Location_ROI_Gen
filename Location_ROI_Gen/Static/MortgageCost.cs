using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location_ROI_Gen.Static
{
    internal static class MortgageCost
    {
        public static Dictionary<int, int> Cost = new Dictionary<int, int>()
        {
            { 90000, 471 },
            { 100000, 523 },
            { 110000, 576 },
            { 120000, 628 },
            { 130000, 680 },
            { 140000, 733 },
            { 150000, 785 },
            { 160000, 837 },
            { 170000, 890 },
            { 180000, 942 },
            { 190000, 995 },
            { 200000, 1047 },
            { 210000, 1099 },
        };

        public static int MortgageCostForHousePrice(int housePrice)
        {
            if (housePrice <= 90000)
            {
                return Cost[90000];
            }
            else if (housePrice <= 100000)
            {
                return Cost[100000];
            }
            else if (housePrice <= 110000)
            {
                return Cost[110000];
            }
            else if (housePrice <= 120000)
            {
                return Cost[120000];
            }
            else if (housePrice <= 130000)
            {
                return Cost[130000];
            }
            else if (housePrice <= 140000)
            {
                return Cost[140000];
            }
            else if (housePrice <= 150000)
            {
                return Cost[150000];
            }
            else if (housePrice <= 160000)
            {
                return Cost[160000];
            }
            else if (housePrice <= 170000)
            {
                return Cost[170000];
            }
            else if (housePrice <= 180000)
            {
                return Cost[180000];
            }
            else if (housePrice <= 190000)
            {
                return Cost[190000];
            }
            else if (housePrice <= 200000)
            {
                return Cost[200000];
            }
            else if (housePrice <= 210000)
            {
                return Cost[210000];
            }
            else
            {
                return 2000;
            }
            return 2000;
        }
    }
}
