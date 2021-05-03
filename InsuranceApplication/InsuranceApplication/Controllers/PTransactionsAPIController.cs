using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InsuranceApplication.Data;
using InsuranceApplication.Models;

namespace InsuranceApplication.Controllers
{
    [Route("api/PTransactionsAPI")]
    [ApiController]
    public class PTransactionsAPIController : ControllerBase
    {
        private readonly TransactionContext _transactionContext;
        private readonly PolicyHolderContext _policyHolderContext;

        public PTransactionsAPIController(TransactionContext context, PolicyHolderContext policyHolderContext)
        {
            _transactionContext = context;
            _policyHolderContext = policyHolderContext;
        }

        // GET: api/PTransactionsAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PTransaction>>> GetPTransactions()
        {
            return await _transactionContext.PTransactions.ToListAsync();
        }

        // GET: api/PTransactionsAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PTransaction>> GetPTransaction(int id)
        {
            var pTransaction = await _transactionContext.PTransactions.FindAsync(id);

            if (pTransaction == null)
            {
                return NotFound();
            }

            return pTransaction;
        }

        // PUT: api/PTransactionsAPI/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPTransaction(int id, PTransaction pTransaction)
        {
            if (id != pTransaction.Id)
            {
                return BadRequest();
            }

            _transactionContext.Entry(pTransaction).State = EntityState.Modified;

            try
            {
                await _transactionContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PTransactionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/PTransactionsAPI
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<PTransaction>> PostPTransaction(PTransaction pTransaction)
        {
            PolicyHolder ph = _policyHolderContext.PolicyHolders.FirstOrDefault(p => p.Name == pTransaction.HolderName);
            if(ph == null)
            {
                return NotFound();
            }
            pTransaction.HolderId = ph.Id;
            _transactionContext.PTransactions.Add(pTransaction);
            await _transactionContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPTransaction), new { id = pTransaction.Id }, pTransaction);
        }

        // DELETE: api/PTransactionsAPI/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PTransaction>> DeletePTransaction(int id)
        {
            var pTransaction = await _transactionContext.PTransactions.FindAsync(id);
            if (pTransaction == null)
            {
                return NotFound();
            }

            _transactionContext.PTransactions.Remove(pTransaction);
            await _transactionContext.SaveChangesAsync();

            return pTransaction;
        }

        private bool PTransactionExists(int id)
        {
            return _transactionContext.PTransactions.Any(e => e.Id == id);
        }
    }
}
