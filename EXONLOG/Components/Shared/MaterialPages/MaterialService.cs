using EXONLOG.Model.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EXONLOG.Components.Shared
{
    public class MaterialService
    {
        private readonly DbContext _context;

        public MaterialService(DbContext context)
        {
            _context = context;
        }

        // Method to get the available quantity of a material
        public async Task<double> GetAvailableQuantityAsync(int materialId)
        {
            var material = await _context.Set<Material>()
                                          .Include(m => m.Stocks) // Ensure related Stocks are loaded
                                          .FirstOrDefaultAsync(m => m.MaterialID == materialId);

            return material?.AvailableQuantity ?? 0; // Return available quantity or 0 if not found
        }

        // Method to get a specific material's details
        public async Task<Material> GetMaterialDetailsAsync(int materialId)
        {
            var material = await _context.Set<Material>()
                                          .Include(m => m.Stocks) // Optional: Include Stocks if needed
                                          .FirstOrDefaultAsync(m => m.MaterialID == materialId);

            return material;
        }

        // Method to get all materials
        public async Task<IQueryable<Material>> GetAllMaterialsAsync()
        {
            return _context.Set<Material>().AsQueryable();
        }

        // Method to add a new material
        public async Task AddMaterialAsync(Material material)
        {
            await _context.Set<Material>().AddAsync(material);
            await _context.SaveChangesAsync();
        }

        // Method to update an existing material
        public async Task UpdateMaterialAsync(Material material)
        {
            _context.Set<Material>().Update(material);
            await _context.SaveChangesAsync();
        }

        // Method to delete a material
        public async Task DeleteMaterialAsync(int materialId)
        {
            var material = await _context.Set<Material>().FindAsync(materialId);
            if (material != null)
            {
                _context.Set<Material>().Remove(material);
                await _context.SaveChangesAsync();
            }
        }
    }
}
