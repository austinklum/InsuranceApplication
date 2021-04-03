using InsuranceApplication.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace InsuranceApplication.Data
{
    public class TransactionContext : DbContext
    {
        public TransactionContext(DbContextOptions<TransactionContext> options) : base(options)
        {

        }

        public DbSet<PTransaction> PTransactions { get; set; }
        public DbSet<Subtransaction> Subtransactions { get; set; }
    }
}
