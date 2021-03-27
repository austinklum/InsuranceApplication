using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceApplication.Models
{
    public class Policy
    {
        public int Id { get; set; }
        [DisplayName("Policy Code")]
        public string PolicyCode { get; set; }
        public string Name { get; set; }
        [DisplayName("Age Limit")]
        public int AgeLimit { get; set; }
        [DisplayName("Maximum Coverage")]
        public double MaxCoverage { get; set; }
        [DisplayName("Percent Coverage")]
        public double PercentCoverage { get; set; }
        public double Premium { get; set; }
        //[DisplayName("Covered Drugs")]
        //public Drug[] CoveredDrugs { get; set; }
    }
}
