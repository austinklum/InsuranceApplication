using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceApplication.Models
{
    public class PrescribedDrug
    {
        public int Id { get; set; }
        public int DrugId { get; set; }
        public int PrescriptionId { get; set; }
        public int Count { get; set; }
        public string Dosage { get; set; }
        public int RefillCount { get; set; }
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public double CoveredAmount { get; set; }
    }
}
