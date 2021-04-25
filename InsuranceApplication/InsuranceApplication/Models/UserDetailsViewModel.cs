using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceApplication.Models
{
    public class UserDetailsViewModel
    {
        public Agent CurrentAgent { get; set; }
        public User CurrentUser { get; set; }
        public IEnumerable<SelectListItem> Questions { get; set; }
    }
}
