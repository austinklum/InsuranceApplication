using InsuranceApplication.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceApplication.Data
{
    public class PolicyHolderContext : DbContext
    {
        public PolicyHolderContext(DbContextOptions<PolicyHolderContext> options) : base(options)
        {
        }

        public DbSet<PolicyHolder> PolicyHolders { get; set; }
    }
}
