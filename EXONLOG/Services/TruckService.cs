namespace EXONLOG.Services
{
    using EXONLOG.Data;
    using EXONLOG.Model.Trans;
    using Microsoft.EntityFrameworkCore;

    
        public class TruckService
        {
            private readonly EXONContext _dbContext;

            public TruckService(EXONContext dbContext)
            {
                _dbContext = dbContext;
            }

            // Get all trucks
            public  Task<List<Truck>> GetAllTrucksAsync()
            {
                return  _dbContext.Set<Truck>()
                                       .OrderBy(t => t.TruckNumber)
                                       .ToListAsync();
            }
        // Search trucks by truck number
        public async Task<List<Truck>> SearchTrucksAsync(string searchTerm)
        {
            return await _dbContext.Trucks
                .Include(t => t.User)
                .Where(t => string.IsNullOrEmpty(searchTerm) ||
                    t.TruckNumber.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .OrderBy(t => t.TruckNumber)
                .Take(10) // Limit results for performance
                .ToListAsync();
        }

        // Get a single truck by ID
        public async Task<Truck?> GetTruckByIdAsync(int truckId)
            {
                return await _dbContext.Set<Truck>()
                                       .FirstOrDefaultAsync(t => t.TruckID == truckId);
            }

            // Get a single truck by ID
            public async Task<Truck?> searchTruck(string tnum)
            {
                return await _dbContext.Set<Truck>()
                                       .FindAsync(tnum);
            }

            // Add a new truck
            public async Task AddTruckAsync(Truck truck)
            {
            truck.UserID = 1;

                truck.CreateDate = DateTime.UtcNow; // Ensure creation date is set
                _dbContext.Set<Truck>().Add(truck);
                await _dbContext.SaveChangesAsync();
            }

            // Update an existing truck
            public async Task UpdateTruckAsync(Truck truck)
            {
                var existingTruck = await GetTruckByIdAsync(truck.TruckID);
                if (existingTruck == null) throw new Exception("Truck not found.");

                // Update fields
                existingTruck.TruckNumber = truck.TruckNumber;
                existingTruck.LicenseNumber = truck.LicenseNumber;
                existingTruck.TrailerNumber = truck.TrailerNumber;
                existingTruck.TrailerLicenseNumber = truck.TrailerLicenseNumber;
                existingTruck.OwnerName = truck.OwnerName;
                existingTruck.TruckType = truck.TruckType;
                existingTruck.Capacity = truck.Capacity;
                existingTruck.Notes = truck.Notes;

                await _dbContext.SaveChangesAsync();
            }

            // Delete a truck
            public async Task DeleteTruckAsync(int truckId)
            {
                var truck = await GetTruckByIdAsync(truckId);
                if (truck == null) throw new Exception("Truck not found.");

                _dbContext.Set<Truck>().Remove(truck);
                await _dbContext.SaveChangesAsync();
            }
        }
    

}
