using EXONLOG.Data;
using EXONLOG.Model.Inbound;
using Microsoft.EntityFrameworkCore;

namespace EXONLOG.Services
{
    public class InLadingService
    {
        private readonly EXONContext _context;

        public InLadingService(EXONContext context)
        {
            _context = context;
        }

        public async Task<List<InLading>> GetAllInLadingsAsync(string searchTerm = "")
        {
            return await _context.InLadings
                .Include(i => i.Batch).ThenInclude(s=>s.Shipment).ThenInclude(m=>m.Material)
                .Include(i => i.Truck)
                .Include(i => i.Driver)
                .Include(i => i.TransCompany)
                .Where(i => string.IsNullOrEmpty(searchTerm) ||
                    i.Batch.BatchNumber.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task CreateInLadingAsync(InLading inLading)
        {
            inLading.CreateDate = DateTime.UtcNow;
            inLading.UserID = 1; // Hardcoded until auth implemented
            _context.InLadings.Add(inLading);
            await _context.SaveChangesAsync();
        }

        // Get OutLading by ID
        public  InLading GetInLadingByIdAsync(int id)
        {
            return  _context.InLadings
                .Include(o => o.Truck)
                .Include(o => o.Driver)
                .Include(o => o.Batch)?.ThenInclude(b=>b.Shipment)?.ThenInclude(s=>s.Material)
                .Include(o => o.FirstWeigher)
                .Include(o => o.SecondWeigher)
                .Include(o => o.User)
                .Include(o => o.TransCompany)
                .FirstOrDefault(o => o.InladID == id);
        }

        // Update an existing IntLading
        public async Task UpdateInLadingAsync(InLading inlading)
        {
            _context.InLadings.Update(inlading);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes an InLading record by its ID.
        /// </summary>
        /// <param name="inLadingId">The ID of the InLading to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DeleteInLadingAsync(int inLadingId)
        {
            // Find the InLading record by ID
            var inLading = await _context.InLadings
                .FirstOrDefaultAsync(i => i.InladID == inLadingId);

            if (inLading == null)
            {
                throw new KeyNotFoundException($"InLading with ID {inLadingId} not found.");
            }

            // Remove the InLading from the database
            _context.InLadings.Remove(inLading);
            await _context.SaveChangesAsync();
        }
        public async Task<List<InLading>> GetInladingsByBatchAsync(int batchId)
        {
            return await _context.InLadings
                .Include(i => i.Batch)
                .Where(i => i.BatchID == batchId)
                .ToListAsync();
        }

    }
}
