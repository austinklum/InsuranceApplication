using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InsuranceApplication.Data;
using InsuranceApplication.Models;

namespace InsuranceApplication.Views.Policies
{
    public class PoliciesController : Controller
    {
        private readonly PolicyContext _context;

        public PoliciesController(PolicyContext context)
        {
            _context = context;
        }

        // GET: Policies
        public async Task<IActionResult> Index(string searchString)
        {
            var policies = from p in _context.Policies select p;
            if(!string.IsNullOrEmpty(searchString))
            {
                policies = policies.Where(s => s.Name.Contains(searchString) || s.PolicyCode.Contains(searchString));
            }
            return View(await policies.ToListAsync());
        }

        // GET: Policies/Details/5
        // put drug list here
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var policy = await _context.Policies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (policy == null)
            {
                return NotFound();
            }

            return View(policy);
        }

        private bool PolicyExists(int id)
        {
            return _context.Policies.Any(e => e.Id == id);
        }


    }
}
