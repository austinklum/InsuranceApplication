using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceApplication.Models
{
    public class PTransaction
    {
        public int Id { get; set; }
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Date { get; set; }
        public int HolderId { get; set; }


        [NotMapped]
        [DisplayName("Policyholder Name")]
        public string HolderName { get; set; }
        [NotMapped]
        [DisplayName("Total Cost of Transaction")]
        public double TotalCost { get; set; }
        [NotMapped]
        [DisplayName("Total Cost without Insurance")]
        public double TotalCostNoIns { get; set; }
        [NotMapped]
        [DisplayName("Status")]
        public bool? Processed { get; set; }
    }
}
