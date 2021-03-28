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
        public async Task<IActionResult> Index()
        {
            return View(await _context.PTransactions.ToListAsync());
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
