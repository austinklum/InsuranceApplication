using InsuranceApplication.Data;
using InsuranceApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceApplication.Components
{
    public class TransactionListViewComponent : ViewComponent
    {
        private readonly TransactionContext _transactionContext;
        private readonly PolicyHolderContext _policyHolderContext;

        public TransactionListViewComponent(TransactionContext transactionContext, PolicyHolderContext policyHolderContext)
        {
            _transactionContext = transactionContext;
            _policyHolderContext = policyHolderContext;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<PTransaction> transactions = (from p in _transactionContext.PTransactions select p).ToList();

            foreach (PTransaction transaction in transactions)
            {
                transaction.Processed = isTransactionProcessed(transaction);
            }
            
            transactions = transactions.Where(t => t.Processed != true).ToList();

            IQueryable<PolicyHolder> holders = from h in _policyHolderContext.PolicyHolders select h;
            List<int> holderIds = (from p in _transactionContext.PTransactions orderby p.HolderId select p.HolderId).ToList();
            holders = holders.Where(h => holderIds.Contains(h.Id));

            IQueryable<string> holderNames = holders.Select(h => h.Name);

            foreach (PTransaction transaction in transactions)
            {
                PolicyHolder policyHolder = await _policyHolderContext.PolicyHolders.FirstAsync(p => p.Id == transaction.HolderId);

                transaction.HolderName = holders.FirstOrDefault(h => h.Id == transaction.HolderId).Name;
            }
            
            HttpContext.Session.SetString("holderName", "");

            HttpContext.Session.SetString("includeProcessed", false.ToString());
            PTransactionsByPolicyHolderViewModel TransactionByPH = new PTransactionsByPolicyHolderViewModel
            {
                Holders = new SelectList(await holderNames.ToListAsync()),
                Transactions = transactions,
            };

            return View(TransactionByPH);
        }

        private bool? isTransactionProcessed(PTransaction transaction, Subtransaction subtransaction = null)
        {
            List<Subtransaction> allSubtransactions = _transactionContext.Subtransactions.Where(s => s.PTransactionId == transaction.Id).ToList();
            bool anyProcessed = false;
            bool allProcessed = true;
            foreach (Subtransaction s in allSubtransactions)
            {
                if (subtransaction != null && s.Id == subtransaction.Id)
                {
                    continue;
                }
                if (s.Accepted != null)
                {
                    anyProcessed = true;
                }
                else
                {
                    allProcessed = false;
                }
            }

            if (!anyProcessed) return false;
            if (anyProcessed && !allProcessed) return null;
            //else allProcessed = true
            return true;
        }
    }
}
