using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InsuranceApplication.Data;
using InsuranceApplication.Models;

namespace InsuranceApplication.Views.PTransactions
{
    public class PTransactionsController : Controller
    {
        private readonly PTransactionContext _pTransactionContext;
        private readonly PolicyHolderContext _policyHolderContext;

        public PTransactionsController(PTransactionContext pTransactionContext, PolicyHolderContext policyHolderContext)
        {
            _pTransactionContext = pTransactionContext;
            _policyHolderContext = policyHolderContext;
        }

        // GET: PTransactions
        public async Task<IActionResult> Index(string holderName, bool includeAccepted)
        {

            IQueryable<PTransaction> transactions = from p in _pTransactionContext.PTransactions select p;

            if(!includeAccepted)
            {
                transactions = transactions.Where(t => !t.Accepted);
            }

            if (!string.IsNullOrEmpty(holderName))
            {
                PolicyHolder p = _policyHolderContext.PolicyHolders.FirstOrDefault(p => p.Name == holderName);
                transactions = transactions.Where(t => t.HolderId == p.Id);
            }

            IQueryable<PolicyHolder> holders = from h in _policyHolderContext.PolicyHolders select h;
            List<int> holderIds = (from p in _pTransactionContext.PTransactions orderby p.HolderId select p.HolderId).ToList();
            holders = holders.Where(h => holderIds.Contains(h.Id));

            IQueryable<string> holderNames = holders.Select(h => h.Name);

            foreach(PTransaction transaction in transactions)
            {
                transaction.HolderName = holders.FirstOrDefault(h => h.Id == transaction.Id).Name;
            }

            PTransactionsByPolicyHolderViewModel TransactionByPH = new PTransactionsByPolicyHolderViewModel
            {
                Holders = new SelectList(await holderNames.ToListAsync()),
                Transactions = await transactions.ToListAsync(),
            };
            return View(TransactionByPH);
        }

        // GET: PTransactions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pTransaction = await _pTransactionContext.PTransactions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pTransaction == null)
            {
                return NotFound();
            }

            return View(pTransaction);
        }

        private bool PTransactionExists(int id)
        {
            return _pTransactionContext.PTransactions.Any(e => e.Id == id);
        }

        public string GetNameFromUserID(int id)
        {
            return _policyHolderContext.PolicyHolders.FirstOrDefault(h => h.Id == id).Name;
        }
    }
}
