namespace EXONLOG.Services
{
    using EXONLOG.Data;
    using EXONLOG.Model.Trans;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

  
        public class DriverService
        {
            private readonly EXONContext _dbContext;

            public DriverService(EXONContext dbContext)
            {
                _dbContext = dbContext;
            }

            // Get all drivers
            public async Task<List<Driver>> GetAllDriversAsync()
            {
                return await _dbContext.Drivers
                    .Include(d => d.User) // Optionally include related user data
                    .ToListAsync();
            }

            // Get a driver by ID
            public async Task<Driver> GetDriverByIdAsync(int driverId)
            {
                return await _dbContext.Drivers
                    .Include(d => d.User)
                    .FirstOrDefaultAsync(d => d.DriverID == driverId);
            }

            // Add a new driver
            public async Task AddDriverAsync(Driver driver)
            {
                _dbContext.Drivers.Add(driver);
                await _dbContext.SaveChangesAsync();
            }

            // Update an existing driver
            public async Task UpdateDriverAsync(Driver driver)
            {
                _dbContext.Drivers.Update(driver);
                await _dbContext.SaveChangesAsync();
            }

            // Delete a driver by ID
            public async Task DeleteDriverAsync(int driverId)
            {
                var driver = await _dbContext.Drivers.FindAsync(driverId);
                if (driver != null)
                {
                    _dbContext.Drivers.Remove(driver);
                    await _dbContext.SaveChangesAsync();
                }
            }
        
    }

}
