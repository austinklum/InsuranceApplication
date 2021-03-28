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
        private readonly PTransactionContext _context;

        public PTransactionsController(PTransactionContext context)
        {
            _context = context;
        }

        // GET: PTransactions
        public async Task<IActionResult> Index(int holderId, bool includeAccepted)
        {
            IQueryable<int> holderQuery = from p in _context.PTransactions orderby p.HolderId select p.HolderId;

            var transactions = from p in _context.PTransactions select p;

            if(!includeAccepted)
            {
                transactions = transactions.Where(t => !t.Accepted);
            }

            if (holderId != 0)
            {
                transactions = transactions.Where(t => t.HolderId == holderId);
            }

            PTransactionsByPolicyHolderViewModel TransactionByPH = new PTransactionsByPolicyHolderViewModel
            {
                Holders = new SelectList(await holderQuery.Distinct().ToListAsync()),
                Transactions = await transactions.ToListAsync()
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

            var pTransaction = await _context.PTransactions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pTransaction == null)
            {
                return NotFound();
            }

            return View(pTransaction);
        }

        private bool PTransactionExists(int id)
        {
            return _context.PTransactions.Any(e => e.Id == id);
        }
    }
}
