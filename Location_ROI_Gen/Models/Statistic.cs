using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location_ROI_Gen.Models
{
    public class Statistic
    {
        public int CouncilRented { get; set; }

        public int Owned { get; set; }

        public int PrivateRented { get; set; }

        public int FamilyHouseholds { get; set; }

        /// <summary>
        /// Percentage of households that are not deprived
        /// </summary>
        public int NotDeprived { get; set; }

        /// <summary>
        /// Rate of deprivation is a score from 0-100, 0 being the least deprived and 100 being the most deprived
        /// </summary>
        public int RateOfDeprivation { get; set; }

        /// <summary>
        /// Percentage of Religious households in a neighbourhood
        /// </summary>
        public int Religious { get; set; }

        /// <summary>
        /// Percentage of employed, ft, part time of self
        /// </summary>
        public int Employed { get; set; }

        /// <summary>
        /// Percentage of unemployed, long term sick
        /// </summary>
        public int Unemployed { get; set; }

        /// <summary>
        /// Percentage of retired people in a neighbourhood
        /// </summary>
        public int Retired { get; set; }

        public int SocioEconomic_High { get; set; }

        public int SocioEconomic_Mid { get; set; }

        public int SocioEconomic_Low { get; set; }
    }
}
