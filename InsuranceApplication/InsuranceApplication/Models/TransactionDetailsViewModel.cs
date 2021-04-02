using InsuranceApplication.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceApplication.Models
{
    public class TransactionDetailsViewModel
    {
        public TransactionDetailsViewModel(PTransaction t, List<Subtransaction> s, PolicyHolder ph, Policy p)
        {
            transaction = t;
            subtransactions = s;
            policyHolder = ph;
            policy = p;
        }
        public PTransaction transaction;
        public List<Subtransaction> subtransactions;
        public PolicyHolder policyHolder;
        public Policy policy;
    }
}
