using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InsuranceApplication.Data;
using InsuranceApplication.Models;
using InsuranceApplication.Controllers;
using Microsoft.AspNetCore.Http;

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

            HttpContext.Session.SetString(HomeController.CreatePolicyHolderNameValidation, "");
            HttpContext.Session.SetString(HomeController.CreatePolicyHolderAddressValidation, "");
            HttpContext.Session.SetString(HomeController.CreatePolicyHolderPronounsValidation, "");
            HttpContext.Session.SetString(HomeController.CreatePolicyHolderPolicyCodeValidation, "");
            HttpContext.Session.SetString(HomeController.CreatePolicyHolderDOBValidation, "");
            HttpContext.Session.SetString(HomeController.CreatePolicyHolderStartValidation, "");
            HttpContext.Session.SetString(HomeController.CreatePolicyHolderEndValidation, "");
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
            HttpContext.Session.SetString(HomeController.CreatePolicyHolderNameValidation, "");
            HttpContext.Session.SetString(HomeController.CreatePolicyHolderAddressValidation, "");
            HttpContext.Session.SetString(HomeController.CreatePolicyHolderPronounsValidation, "");
            HttpContext.Session.SetString(HomeController.CreatePolicyHolderPolicyCodeValidation, "");
            HttpContext.Session.SetString(HomeController.CreatePolicyHolderDOBValidation, "");
            HttpContext.Session.SetString(HomeController.CreatePolicyHolderStartValidation, "");
            HttpContext.Session.SetString(HomeController.CreatePolicyHolderEndValidation, "");

            var codes = from p in _policyContext.Policies select p.PolicyCode;
            vm.Policies = GetSelectListItems(codes);
            if (string.IsNullOrEmpty(vm.CurrentPolicyHolder.Name))
            {
                HttpContext.Session.SetString(HomeController.CreatePolicyHolderNameValidation, "Required Field");
                return View(vm);
            }
            if (vm.CurrentPolicyHolder.DateOfBirth == DateTime.MinValue)
            {
                HttpContext.Session.SetString(HomeController.CreatePolicyHolderDOBValidation, "Required Field");
                return View(vm);
            }
            if (string.IsNullOrEmpty(vm.CurrentPolicyHolder.Pronouns))
            {
                HttpContext.Session.SetString(HomeController.CreatePolicyHolderPronounsValidation, "Required Field");
                return View(vm);
            }
            if (string.IsNullOrEmpty(vm.CurrentPolicyHolder.Address))
            {
                HttpContext.Session.SetString(HomeController.CreatePolicyHolderAddressValidation, "Required Field");
                return View(vm);
            }
            if (string.IsNullOrEmpty(vm.CurrentPolicyHolder.PolicyCode))
            {
                HttpContext.Session.SetString(HomeController.CreatePolicyHolderPolicyCodeValidation, "Required Field");
                return View(vm);
            }
            if (vm.CurrentPolicyHolder.StartDate == DateTime.MinValue)
            {
                HttpContext.Session.SetString(HomeController.CreatePolicyHolderStartValidation, "Required Field");
                return View(vm);
            }
            if (vm.CurrentPolicyHolder.EndDate == DateTime.MinValue)
            {
                HttpContext.Session.SetString(HomeController.CreatePolicyHolderEndValidation, "Required Field");
                return View(vm);
            }
            PolicyHolder policyHolder = vm.CurrentPolicyHolder;
            policyHolder.AmountPaid = 0;
            Policy policy = _policyContext.Policies.First(p => p.PolicyCode == policyHolder.PolicyCode);
            DateTime currentYear = new DateTime(DateTime.Now.Year, 1, 1);
            if (vm.CurrentPolicyHolder.DateOfBirth >= currentYear)
            {
                HttpContext.Session.SetString(HomeController.CreatePolicyHolderDOBValidation, "Policyholder cannot be born in this year");
                return View(vm);
            }
            double years = (DateTime.Now - vm.CurrentPolicyHolder.DateOfBirth).TotalDays / 365.25;
            if (years > policy.AgeLimit)
            {
                HttpContext.Session.SetString(HomeController.CreatePolicyHolderDOBValidation, "Policyholder is too old for this policy");
                return View(vm);
            }
            if (vm.CurrentPolicyHolder.StartDate < DateTime.Now)
            {
                HttpContext.Session.SetString(HomeController.CreatePolicyHolderStartValidation, "Start date cannot be before today");
                return View(vm);
            }
            if (vm.CurrentPolicyHolder.StartDate > vm.CurrentPolicyHolder.EndDate)
            {
                HttpContext.Session.SetString(HomeController.CreatePolicyHolderEndValidation, "End date cannot be before start date");
                return View(vm);
            }
            policyHolder.AmountRemaining = policy.MaxCoverage;
            _policyHolderContext.Add(policyHolder);
            await _policyHolderContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        private IEnumerable<SelectListItem> GetSelectListItems(IEnumerable<string> elements)
        {
            var selectList = new List<SelectListItem>();
            for (int i = 0; i < elements.Count(); i++)
            {
                selectList.Add(new SelectListItem
                {
                    Value = elements.ElementAt(i),
                    Text = elements.ElementAt(i)
                }); ;
            }

            return selectList;
        }
    }
}
