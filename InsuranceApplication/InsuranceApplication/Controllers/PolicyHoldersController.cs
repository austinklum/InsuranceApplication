using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InsuranceApplication.Data;
using InsuranceApplication.Models;

namespace InsuranceApplication.Views.PolicyHolders
{
    public class PolicyHoldersController : Controller
    {
        private readonly PolicyHolderContext _context;

        public PolicyHoldersController(PolicyHolderContext context)
        {
            _context = context;
        }

        // GET: PolicyHolders
        public async Task<IActionResult> Index(string searchString)
        {
            var holders = from p in _context.PolicyHolders select p;
            if (!string.IsNullOrEmpty(searchString))
            {
                holders = holders.Where(s => s.Name.Contains(searchString) || s.PolicyCode.Contains(searchString));
            }
            return View(await holders.ToListAsync());
        }

        // GET: PolicyHolders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var policyHolder = await _context.PolicyHolders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (policyHolder == null)
            {
                return NotFound();
            }

            return View(policyHolder);
        }

        private bool PolicyHolderExists(int id)
        {
            return _context.PolicyHolders.Any(e => e.Id == id);
        }
    }
}
