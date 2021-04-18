using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InsuranceApplication.Data;

namespace InsuranceApplication.Controllers
{
    [Route("api/SubtransactionsAPI")]
    [ApiController]
    public class SubtransactionsAPIController : ControllerBase
    {
        private readonly TransactionContext _context;

        public SubtransactionsAPIController(TransactionContext context)
        {
            _context = context;
        }

        // GET: api/SubtransactionsAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Subtransaction>>> GetSubtransactions()
        {
            return await _context.Subtransactions.ToListAsync();
        }

        // GET: api/SubtransactionsAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Subtransaction>> GetSubtransaction(int id)
        {
            var subtransaction = await _context.Subtransactions.FindAsync(id);

            if (subtransaction == null)
            {
                return NotFound();
            }

            return subtransaction;
        }

        // PUT: api/SubtransactionsAPI/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSubtransaction(int id, Subtransaction subtransaction)
        {
            if (id != subtransaction.Id)
            {
                return BadRequest();
            }

            _context.Entry(subtransaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubtransactionExists(id))
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

        // POST: api/SubtransactionsAPI
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Subtransaction>> PostSubtransaction(Subtransaction subtransaction)
        {
            _context.Subtransactions.Add(subtransaction);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSubtransaction), new { id = subtransaction.Id }, subtransaction);
        }

        // DELETE: api/SubtransactionsAPI/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Subtransaction>> DeleteSubtransaction(int id)
        {
            var subtransaction = await _context.Subtransactions.FindAsync(id);
            if (subtransaction == null)
            {
                return NotFound();
            }

            _context.Subtransactions.Remove(subtransaction);
            await _context.SaveChangesAsync();

            return subtransaction;
        }

        private bool SubtransactionExists(int id)
        {
            return _context.Subtransactions.Any(e => e.Id == id);
        }
    }
}
