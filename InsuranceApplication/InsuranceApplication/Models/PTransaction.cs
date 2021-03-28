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
        [DisplayName("Drug")]
        public string DrugCode { get; set; }
        [NotMapped]
        public string DrugName { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date { get; set; }
        [DisplayName("Amount Paid")]
        public double AmountPaid { get; set; }
        public int HolderId { get; set; }
        public int Count { get; set; }
        [NotMapped]
        [DisplayName("Policyholder Name")]
        public string HolderName { get; set; }
        public bool Accepted { get; set; }
    }
}
