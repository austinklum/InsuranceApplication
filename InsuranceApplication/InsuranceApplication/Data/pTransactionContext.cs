using InsuranceApplication.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace InsuranceApplication.Data
{
    public class PTransactionContext : DbContext
    {
        public PTransactionContext(DbContextOptions<PTransactionContext> options) : base(options)
        {

        }

        public DbSet<PTransaction> PTransactions { get; set; }
    }
}
