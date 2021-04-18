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
using System.Text.Json;
using System.Text;
using System.Net;
using System.IO;

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

            List<PTransaction> transactions = (from p in _transactionContext.PTransactions select p).ToList();

            foreach (PTransaction transaction in transactions)
            {
                transaction.Processed = isTransactionProcessed(transaction);
            }

            if (!includeProcessed)
            {
                transactions = transactions.Where(t => t.Processed != true).ToList();
            }

            if (!string.IsNullOrEmpty(holderName))
            {
                PolicyHolder p = _policyHolderContext.PolicyHolders.FirstOrDefault(p => p.Name == holderName);
                transactions = transactions.Where(t => t.HolderId == p.Id).ToList();
            }

            IQueryable<PolicyHolder> holders = from h in _policyHolderContext.PolicyHolders select h;
            List<int> holderIds = (from p in _transactionContext.PTransactions orderby p.HolderId select p.HolderId).ToList();
            holders = holders.Where(h => holderIds.Contains(h.Id));


            IQueryable<string> holderNames = holders.Select(h => h.Name);

            foreach (PTransaction transaction in transactions)
            {
                PolicyHolder policyHolder = await _policyHolderContext.PolicyHolders.FirstAsync(p => p.Id == transaction.HolderId);

                transaction.HolderName = holders.FirstOrDefault(h => h.Id == transaction.HolderId).Name;
            }

            if (!string.IsNullOrEmpty(holderName))
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
                Transactions = transactions,
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

            PTransaction transaction = await _transactionContext.PTransactions.FirstAsync(m => m.Id == id);
            PolicyHolder policyHolder = await _policyHolderContext.PolicyHolders.FirstAsync(p => p.Id == transaction.HolderId);
            Policy policy = await _policyContext.Policies.FirstAsync(p => p.Id == policyHolder.Id);

            IQueryable<Subtransaction> subtransactions = _transactionContext.Subtransactions.Where(s => s.PTransactionId == transaction.Id);
            foreach (Subtransaction s in subtransactions)
            {
                s.CurrentDrug = await _drugContext.Drugs.FirstAsync(d => d.Id == s.DrugId);

            }

            TransactionDetailsViewModel vm = new TransactionDetailsViewModel(transaction, subtransactions.ToList(), policyHolder, policy);

            return View(vm);
        }

        // GET: PTransactions/Details/5
        public async Task<IActionResult> Process(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Subtransaction subtransaction = await _transactionContext.Subtransactions.FirstAsync(s => s.Id == id);

            PTransaction transaction = await _transactionContext.PTransactions.FirstAsync(m => m.Id == subtransaction.PTransactionId);

            PolicyHolder policyHolder = await _policyHolderContext.PolicyHolders.FirstAsync(p => p.Id == transaction.HolderId);

            Policy policy = await _policyContext.Policies.FirstAsync(p => p.Id == policyHolder.Id);

            Drug drug = await _drugContext.Drugs.FirstAsync(d => d.Id == subtransaction.DrugId);

            IQueryable<PolicyDrug> coveredDrugs = _policyDrugContext.PolicyDrugs.Where(pd => pd.PolicyId == policy.Id);

            PolicyDrug pd = coveredDrugs.FirstOrDefault(cd => cd.DrugId == drug.Id);

            bool inDate = policyHolder.StartDate < DateTime.Now && DateTime.Now < policyHolder.EndDate;
            bool inPolicy = pd != null;
            if (!inDate)
            {
                subtransaction.Accepted = -2;
            }
            else if (!inPolicy)
            {
                subtransaction.Accepted = -1;
            }
            else
            {
                subtransaction.Accepted = 1;
            }

            transaction.Processed = isTransactionProcessed(transaction, subtransaction);

            if (subtransaction.Accepted == 1)
            {
                subtransaction.AmountPaid = getTotalCost(drug, policy, policyHolder, subtransaction);
                policyHolder.AmountPaid += subtransaction.AmountPaid;
                policyHolder.AmountRemaining = policy.MaxCoverage - policyHolder.AmountPaid;

                _policyHolderContext.PolicyHolders.Update(policyHolder);
                _policyHolderContext.SaveChanges();
            }
            _transactionContext.PTransactions.Update(transaction);
            _transactionContext.Subtransactions.Update(subtransaction);
            _transactionContext.SaveChanges();

            // Send response to pharmacy
            SendResponse( subtransaction);

            string holderName = "";
            if (policyHolder.Name == HttpContext.Session.GetString("holderName"))
            {
                holderName = policyHolder.Name;
            }

            bool includeProcessed = false;
            if(!string.IsNullOrEmpty(HttpContext.Session.GetString("includeProcessed")))
            {
                includeProcessed = bool.Parse(HttpContext.Session.GetString("includeProcessed"));
            }

            return RedirectToAction("Details", new { id = transaction.Id });
        }

        public async Task<IActionResult> ProcessAll(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            PTransaction transaction = await _transactionContext.PTransactions.FirstAsync(m => m.Id == id);

            List<Subtransaction> subtransactions = _transactionContext.Subtransactions.Where(s => s.PTransactionId == transaction.Id).ToList();

            foreach (Subtransaction s in subtransactions)
            {
                await Process(s.Id);
            }

            return RedirectToAction("Details", new { id = id });
        }

        private double getTotalCost(Drug d, Policy p, PolicyHolder h, Subtransaction s)
        {
            if (s.Accepted == 1)
            {
                return Math.Min(h.AmountRemaining, Math.Round(d.CostPer * s.Count * p.PercentCoverage, 2));
            }
            return 0;
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
                if (s.Accepted != 0)
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

        private void SendResponse(Subtransaction subtransaction)
        {
            PrescribedDrug pd = new PrescribedDrug
            {
                Id = subtransaction.Id,
                CoveredAmount = subtransaction.AmountPaid,
            };

            string json = JsonSerializer.Serialize(pd);

            var bytes = Encoding.UTF8.GetBytes(json);
#if DEBUG
            var request = (HttpWebRequest)WebRequest.Create("https://localhost:44381/api/PrescribedDrugsAPI");
#else
                var request = (HttpWebRequest)WebRequest.Create("http://wngcsp86.intra.uwlax.edu:8080/api/SubtransactionsAPI");
#endif

            request.Method = "POST";
            request.ContentLength = bytes.Length;
            request.ContentType = "application/json";
            Stream stream = request.GetRequestStream();
            stream.Write(bytes, 0, bytes.Length);
            stream.Close();

            WebResponse subtransactionResponse = request.GetResponse();

        }
    }
}
