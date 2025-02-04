using EXONLOG.Data;
using EXONLOG.Model.Inbound;
using Microsoft.EntityFrameworkCore;

namespace EXONLOG.Services
{
    public class PortService
    {
        private readonly EXONContext _context;

        public PortService(EXONContext context)
        {
            _context = context;
        }

        public async Task<List<Port>> GetPortsAsync(string searchTerm = "")
        {
            return await _context.Ports
                .Include(p => p.User)
                .Where(p => string.IsNullOrEmpty(searchTerm) ||
                    p.PortName.Contains(searchTerm) ||
                    p.Location.Contains(searchTerm))
                .OrderBy(p => p.PortName)
                .ToListAsync();
        }

        public async Task<Port?> GetPortByIdAsync(int id)
        {
            return await _context.Ports
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.PortID == id);
        }

        public async Task AddPortAsync(Port port)
        {
            port.CreateDate = DateTime.UtcNow;
            _context.Ports.Add(port);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePortAsync(Port port)
        {
            _context.Entry(port).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeletePortAsync(int id)
        {
            var port = await _context.Ports.FindAsync(id);
            if (port != null)
            {
                _context.Ports.Remove(port);
                await _context.SaveChangesAsync();
            }
        }
    }
}
