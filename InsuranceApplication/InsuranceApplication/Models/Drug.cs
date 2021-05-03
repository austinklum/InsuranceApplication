using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceApplication.Models
{
    public class Drug
    {
        public int Id { get; set; }
        public string Code { get; set; }
        [DisplayName("Medical Name")]
        public string MedicalName { get; set; }
        [DisplayName("Commercial Name")]
        public string CommercialName { get; set; }
        public string Type { get; set; }
        [DisplayName("Cost Per Dose")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public double CostPer { get; set; }
    }
}
