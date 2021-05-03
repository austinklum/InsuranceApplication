using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceApplication.Models
{
    public class CreatePolicyHolderViewModel
    {
        public PolicyHolder CurrentPolicyHolder { get; set; }
        public IEnumerable<SelectListItem> Policies { get; set; }
    }
}
