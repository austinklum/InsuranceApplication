using InsuranceApplication.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceApplication.Data
{
    public class AgentContext : DbContext
    {
        public AgentContext(DbContextOptions<AgentContext> options) : base(options)
        {

        }

        public DbSet<Agent> Policies { get; set; }
    }
}
