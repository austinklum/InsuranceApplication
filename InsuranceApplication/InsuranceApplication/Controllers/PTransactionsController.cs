using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InsuranceApplication.Data;
using InsuranceApplication.Models;
using Microsoft.AspNetCore.Http;

namespace InsuranceApplication.Views.PTransactions
{
    public class PTransactionsController : Controller
    {
        private readonly TransactionContext _transactionContext;
        private readonly PolicyHolderContext _policyHolderContext;
        private readonly PolicyDrugContext _policyDrugContext;
        private readonly PolicyContext _policyContext;
        private readonly DrugContext _drugContext;

        public PTransactionsController(TransactionContext pTransactionContext, PolicyHolderContext policyHolderContext, PolicyContext policyContext, DrugContext drugContext, PolicyDrugContext policyDrugContext)
        {
            _transactionContext = pTransactionContext;
            _policyHolderContext = policyHolderContext;
            _policyContext = policyContext;
            _drugContext = drugContext;
            _policyDrugContext = policyDrugContext;
        }

        // GET: PTransactions
        public async Task<IActionResult> Index(string holderName, bool includeProcessed)
        {

            IQueryable<PTransaction> transactions = from p in _transactionContext.PTransactions select p;

            if(!includeProcessed)
            {
                transactions = transactions.Where(t => t.Processed != true);
            }

            if (!string.IsNullOrEmpty(holderName))
            {
                PolicyHolder p = _policyHolderContext.PolicyHolders.FirstOrDefault(p => p.Name == holderName);
                transactions = transactions.Where(t => t.HolderId == p.Id);
            }

            IQueryable<PolicyHolder> holders = from h in _policyHolderContext.PolicyHolders select h;
            List<int> holderIds = (from p in _transactionContext.PTransactions orderby p.HolderId select p.HolderId).ToList();
            holders = holders.Where(h => holderIds.Contains(h.Id));

            IQueryable<string> holderNames = holders.Select(h => h.Name);


            //foreach (PTransaction transaction in transactions)
            //{
            //    Drug drug = await _drugContext.Drugs.FirstAsync(d => d.Code == transaction.DrugCode);
            //    PolicyHolder policyHolder = await _policyHolderContext.PolicyHolders.FirstAsync(p => p.Id == transaction.HolderId);
            //    Policy policy = await _policyContext.Policies.FirstAsync(p => p.Id == policyHolder.Id);

            //    transaction.HolderName = holders.FirstOrDefault(h => h.Id == transaction.HolderId).Name;
            //    transaction.DrugName = _drugContext.Drugs.First(d => d.Code == transaction.DrugCode).MedicalName;
            //    transaction.TotalCost = getTotalCost(drug, policy, policyHolder, transaction);

            //}

            if(!string.IsNullOrEmpty(holderName))
            {
                HttpContext.Session.SetString("holderName", holderName);
            }
            else
            {
                HttpContext.Session.SetString("holderName", "");
            }

            HttpContext.Session.SetString("includeProcessed", includeProcessed.ToString());
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

            //PTransaction transaction = await _transactionContext.PTransactions.FirstAsync(m => m.Id == id);
            //Drug drug = await _drugContext.Drugs.FirstAsync(d => d.Code == transaction.DrugCode);
            //PolicyHolder policyHolder = await _policyHolderContext.PolicyHolders.FirstAsync(p => p.Id == transaction.HolderId);
            //Policy policy = await _policyContext.Policies.FirstAsync(p => p.Id == policyHolder.Id);

            //transaction.TotalCost = getTotalCost(drug, policy, policyHolder, transaction);
            //transaction.TotalCostNoIns = Math.Round(drug.CostPer * transaction.Count,2);
            //TransactionDetailsViewModel vm = new TransactionDetailsViewModel(transaction, drug);

            //return View(vm);
            return View();
        }

        // GET: PTransactions/Details/5
        public async Task<IActionResult> Process(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PTransaction transaction = await _transactionContext.PTransactions.FirstAsync(m => m.Id == id);

            PolicyHolder policyHolder = await _policyHolderContext.PolicyHolders.FirstAsync(p => p.Id == transaction.HolderId);

            Policy policy = await _policyContext.Policies.FirstAsync(p => p.Id == policyHolder.Id);

            //Drug drug = await _drugContext.Drugs.FirstAsync(d => d.Code == transaction.DrugCode);

            //IQueryable<PolicyDrug> coveredDrugs = _policyDrugContext.PolicyDrugs.Where(pd => pd.PolicyId == policy.Id);

            //PolicyDrug pd = coveredDrugs.FirstOrDefault(cd => cd.DrugId == drug.Id);

            //bool inDate = policyHolder.StartDate < DateTime.Now && DateTime.Now < policyHolder.EndDate;
            //bool inPolicy = pd != null;

            //transaction.Accepted = inDate && inPolicy;

            //if(transaction.Accepted == true)
            //{
            //    transaction.AmountPaid = getTotalCost(drug, policy, policyHolder, transaction);
            //    policyHolder.AmountPaid += transaction.AmountPaid;
            //    policyHolder.AmountRemaining = policy.MaxCoverage - policyHolder.AmountPaid;

            //    _policyHolderContext.PolicyHolders.Update(policyHolder);
            //    _policyHolderContext.SaveChanges();
            //}
            //_pTransactionContext.PTransactions.Update(transaction);
            //_pTransactionContext.SaveChanges();

            // Send response to pharmacy

            string holderName = "";
            if(policyHolder.Name == HttpContext.Session.GetString("holderName"))
            {
                holderName = policyHolder.Name;
            }
            bool includeProcessed = bool.Parse(HttpContext.Session.GetString("includeProcessed"));

            return RedirectToAction("Index", new { holderName = holderName, includeProcessed = includeProcessed });
        }

        private double getTotalCost(Drug d, Policy p, PolicyHolder h, PTransaction t)
        {
            //if (t.Accepted == true)
            //{
            //    return Math.Min(h.AmountRemaining, Math.Round(d.CostPer * t.Count * p.PercentCoverage, 2));
            //}
            return 0;
        }
    }
}
