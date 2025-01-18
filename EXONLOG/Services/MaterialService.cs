using EXONLOG.Data;
using EXONLOG.Model.Stocks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EXONLOG.Services
{
    public class MaterialService
    {
        private readonly EXONContext _context;

        public MaterialService(EXONContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // Method to get the available quantity of a material
        public async Task<double> GetAvailableQuantityAsync(int materialId)
        {
            var material = await _context.Set<Material>()
                                          .Include(m => m.Stocks) // Ensure related Stocks are loaded
                                          .FirstOrDefaultAsync(m => m.MaterialID == materialId);

            return material?.FreeQuantityInStock ?? 0; // Return available quantity or 0 if not found
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
        public async Task<List<Material>> GetMaterialsAsync()
        {
            return await _context.Materials.ToListAsync();
        }

        public async Task<List<Material>> GetMaterialsWithStockTotalsAsync()
        {
            return await _context.Materials
                .Select(material => new Material
                {
                    MaterialID = material.MaterialID,
                    MaterialName = material.MaterialName,
                    Prefix = material.Prefix,
                    Description = material.Description,
                   Stocks = material.Stocks
                })
                .ToListAsync();
        }

        public async Task<List<Material>> GetMaterialsWithQuantitiesAsync()
        {
            var materials = await _context.Materials
                .Include(m => m.Stocks)
                .Include(m => m.Contracts)
                .ToListAsync();

            // Calculate ReservedQuantity, TotalStockQuantity, FreeQuantityInStock in memory
            foreach (var material in materials)
            {
                material.ReservedQuantity = material.Contracts?.Sum(contract => contract.Quantity) ?? 0;
                material.TotalStockQuantity = material.Stocks?.Sum(stock => stock.Quantity) ?? 0;
                material.FreeQuantityInStock = material.TotalStockQuantity - material.ReservedQuantity;
            }

            return materials;
        }
        // Method to add a new material
        public async Task AddMaterialAsync(Material material)
        {
            material.UserID = 1;
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
        public double GetAvailableQuantity(int materialId)
        {
            var material = _context.Materials
                                    .Include(m => m.Stocks) // Ensure related Stocks are loaded
                                    .FirstOrDefault(m => m.MaterialID == materialId);

            return material?.Stocks.Sum(s => s.Quantity)??0; // Get available quantity or 0 if material not found
        }
    }
}
