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
        private readonly PolicyContext _policyContext;
        private readonly DrugContext _drugContext;

        public PTransactionsController(PTransactionContext pTransactionContext, PolicyHolderContext policyHolderContext, PolicyContext policyContext, DrugContext drugContext)
        {
            _pTransactionContext = pTransactionContext;
            _policyHolderContext = policyHolderContext;
            _policyContext = policyContext;
            _drugContext = drugContext;
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
                transaction.HolderName = holders.FirstOrDefault(h => h.Id == transaction.HolderId).Name;
                transaction.DrugName = _drugContext.Drugs.First(d => d.Code == transaction.DrugCode).MedicalName;
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

        // GET: PTransactions/Details/5
        public async Task<IActionResult> Accept(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PTransaction transaction = await _pTransactionContext.PTransactions
                .FirstAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }
            transaction.Accepted = true;

            Drug drug = await _drugContext.Drugs
                .FirstAsync(d => d.Code == transaction.DrugCode);

            PolicyHolder policyHolder = await _policyHolderContext.PolicyHolders
                .FirstAsync(p => p.Id == transaction.HolderId);

            Policy policy = await _policyContext.Policies
                .FirstAsync(p => p.Id == policyHolder.Id);

            transaction.AmountPaid = Math.Round(drug.CostPer * policy.PercentCoverage,2);
            policyHolder.AmountPaid += transaction.AmountPaid;
            policyHolder.AmountRemaining = policy.MaxCoverage - policyHolder.AmountPaid;

            _policyHolderContext.PolicyHolders.Update(policyHolder);
            _policyHolderContext.SaveChanges();
            _pTransactionContext.PTransactions.Update(transaction);
            _pTransactionContext.SaveChanges();

            // Send response to pharmacy

            return RedirectToAction("Index");
        }
    }
}
