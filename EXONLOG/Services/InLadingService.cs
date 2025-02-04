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
                .Include(i => i.Batch)
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
    }
}
