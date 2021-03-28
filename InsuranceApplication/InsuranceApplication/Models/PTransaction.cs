using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceApplication.Models
{
    public class PTransaction
    {
        public int Id { get; set; }
        public string DrugName { get; set; }
        public DateTime Date { get; set; }
        public double AmountPaid { get; set; }
        public int HolderId { get; set; }
        [NotMapped]
        [DisplayName("Policyholder Name")]
        public string HolderName { get; set; }
        public bool Accepted { get; set; }
    }
}
