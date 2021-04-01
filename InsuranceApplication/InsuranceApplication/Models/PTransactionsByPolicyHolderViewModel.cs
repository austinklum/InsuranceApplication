using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceApplication.Models
{
    public class PTransactionsByPolicyHolderViewModel
    {
        public List<PTransaction> Transactions { get; set; }
        public SelectList Holders { get; set; }
        public string HolderName { get; set; }
        public bool IncludeProcessed { get; set; }
    }
}
