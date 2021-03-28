using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceApplication.Models
{
    public class TransactionDetailsViewModel
    {
        public TransactionDetailsViewModel(PTransaction t, Drug d)
        {
            transaction = t;
            drug = d;
        }
        public PTransaction transaction;
        public Drug drug;
    }
}
