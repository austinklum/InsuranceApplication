using InsuranceApplication.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceApplication.Data
{
    public class PolicyContext : DbContext
    {
        public PolicyContext (DbContextOptions<PolicyContext> options) : base(options)
        {

        }

        public DbSet<Policy> Policies { get; set; }
    }
}
