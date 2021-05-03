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
        private readonly PolicyHolderContext _policyHolderContext;
        private readonly PolicyContext _policyContext;

        public PolicyHoldersController(PolicyHolderContext policyHolderContext, PolicyContext policyContext)
        {
            _policyHolderContext = policyHolderContext;
            _policyContext = policyContext;
        }

        // GET: PolicyHolders
        public async Task<IActionResult> Index(string searchString)
        {
            var holders = from p in _policyHolderContext.PolicyHolders select p;
            if (!string.IsNullOrEmpty(searchString))
            {
                holders = holders.Where(s => s.Name.Contains(searchString) || s.PolicyCode.Contains(searchString));
            }

            foreach (PolicyHolder ph in holders)
            {
                ph.PolicyCode = _policyContext.Policies.First(p => p.Id == ph.PolicyId).PolicyCode;
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

            var policyHolder = await _policyHolderContext.PolicyHolders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (policyHolder == null)
            {
                return NotFound();
            }

            return View(policyHolder);
        }

        private bool PolicyHolderExists(int id)
        {
            return _policyHolderContext.PolicyHolders.Any(e => e.Id == id);
        }

        // GET: Physicians/Create
        public IActionResult Create()
        {
            CreatePolicyHolderViewModel vm = new CreatePolicyHolderViewModel();
            vm.CurrentPolicyHolder = new PolicyHolder();
            var codes = from p in _policyContext.Policies select p.PolicyCode;
            vm.Policies = GetSelectListItems(codes);
            return View(vm);
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(CreatePolicyHolderViewModel vm)
        {
            PolicyHolder policyHolder = vm.CurrentPolicyHolder;
            policyHolder.AmountPaid = 0;
            Policy policy = _policyContext.Policies.First(p => p.Id == policyHolder.PolicyId);
            policyHolder.AmountRemaining = policy.MaxCoverage;
            if (ModelState.IsValid)
            {
                _policyHolderContext.Add(policyHolder);
                await _policyHolderContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(policyHolder);
        }

        private IEnumerable<SelectListItem> GetSelectListItems(IEnumerable<string> elements)
        {
            var selectList = new List<SelectListItem>();
            for (int i = 0; i < elements.Count(); i++)
            {
                selectList.Add(new SelectListItem
                {
                    Value = i.ToString(),
                    Text = elements.ElementAt(i)
                }); ;
            }

            return selectList;
        }
    }
}
