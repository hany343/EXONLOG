using EXONLOG.Data;
using EXONLOG.Model.Inbound;
using Microsoft.EntityFrameworkCore;

namespace EXONLOG.Services
{
    public class BatchService
    {
        private readonly EXONContext _context;

        public BatchService(EXONContext context)
        {
            _context = context;
        }

        public async Task<List<Batch>> GetBatchesAsync(string searchTerm = "")
        {
            return await _context.Batches
                .Include(b => b.Shipment)
                    .ThenInclude(s => s.Material)
                .Include(b => b.User)
                .Where(b => string.IsNullOrEmpty(searchTerm) ||
                    b.BatchNumber.Contains(searchTerm) ||
                    b.Shipment.Material.MaterialName.Contains(searchTerm))
                .OrderByDescending(b => b.CreateDate)
                .ToListAsync();
        }

        public async Task<Batch?> GetBatchByIdAsync(int id)
        {
            return await _context.Batches
                .Include(b => b.Shipment)
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.ID == id);
        }

        public async Task AddBatchAsync(Batch batch)
        {
            batch.CreateDate = DateTime.UtcNow;
            _context.Batches.Add(batch);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBatchAsync(Batch batch)
        {
            _context.Entry(batch).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBatchAsync(int id)
        {
            var batch = await _context.Batches.FindAsync(id);
            if (batch != null)
            {
                _context.Batches.Remove(batch);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<double> GetAvailableQuantityAsync(int shipmentId)
        {
            var shipment = await _context.Shipments
                .Include(s => s.Batches)
                .FirstOrDefaultAsync(s => s.ShipmentID == shipmentId);

            if (shipment == null) return 0;

            var usedQuantity = shipment.Batches.Sum(b => b.Quantity);
            return (shipment.Quantity - usedQuantity);
        }
    }
}
