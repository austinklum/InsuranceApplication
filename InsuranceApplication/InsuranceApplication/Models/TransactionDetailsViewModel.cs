using InsuranceApplication.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceApplication.Models
{
    public class TransactionDetailsViewModel
    {
        public TransactionDetailsViewModel(PTransaction t, List<Subtransaction> s)
        {
            transaction = t;
            subtransactions = s;
        }
        public PTransaction transaction;
        public List<Subtransaction> subtransactions;
    }
}
