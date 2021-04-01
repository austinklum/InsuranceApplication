using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceApplication.Models
{
    [Table("PolicyDrug")]
    public class PolicyDrug
    {
        public int Id { get; set; }
        public int PolicyId { get; set; }
        public int DrugId { get; set; }
    }
}
