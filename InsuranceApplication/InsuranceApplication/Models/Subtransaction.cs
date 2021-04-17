using InsuranceApplication.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceApplication.Data
{
    public class Subtransaction
    {
        public int Id { get; set; }
        public int PTransactionId { get; set; }
        public int DrugId { get; set; }
        public int Count { get; set; }
        public double AmountPaid { get; set; }
        [DisplayName("Status")]
        public int Accepted { get; set; }

        [NotMapped]
        [DisplayName("Drug")]
        public Drug CurrentDrug { get; set; }
        [NotMapped]
        [DisplayName("Total Cost of Drug")]
        public double TotalCost { get; set; }
        [NotMapped]
        [DisplayName("Total Cost without Insurance")]
        public double TotalCostNoIns { get; set; }
    }
}
