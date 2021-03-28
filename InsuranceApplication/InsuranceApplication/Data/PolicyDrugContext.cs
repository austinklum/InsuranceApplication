using InsuranceApplication.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceApplication.Data
{
    public class PolicyDrugContext : DbContext
    {
        public PolicyDrugContext(DbContextOptions<PolicyDrugContext> options) : base(options)
        {

        }

        public DbSet<PolicyDrug> Policies { get; set; }
    }
}
