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
        private readonly PolicyContext _policyContext;
        private readonly PolicyDrugContext _policyDrugContext;
        private readonly DrugContext _drugContext;

        public PoliciesController(PolicyContext pc, PolicyDrugContext pdc,DrugContext dc)
        {
            _policyContext = pc;
            _policyDrugContext = pdc;
            _drugContext = dc;
        }

        // GET: Policies
        public async Task<IActionResult> Index(string searchString)
        {
            var policies = from p in _policyContext.Policies select p;
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

            Policy policy = await _policyContext.Policies.FirstAsync(m => m.Id == id);

            if (policy == null)
            {
                return NotFound();
            }

            List<PolicyDrug> policyDrugs = _policyDrugContext.PolicyDrugs.Where(pd => pd.PolicyId == policy.Id).ToList();

            List<Drug> coveredDrugs = new List<Drug>();

            foreach(PolicyDrug pd in policyDrugs)
            {
                coveredDrugs.AddRange(_drugContext.Drugs.Where(d => d.Id == pd.DrugId));
            }
            
            policy.CoveredDrugs = coveredDrugs;

            return View(policy);
        }

        private bool PolicyExists(int id)
        {
            return _policyContext.Policies.Any(e => e.Id == id);
        }


    }
}
