using EXONLOG.Data;
using EXONLOG.Model.Outbound;
using Microsoft.EntityFrameworkCore;
using System;

namespace EXONLOG.Services
{
    public class OutladingService
    {
        private readonly EXONContext _context;

        public OutladingService(EXONContext context)
        {
            _context = context;
        }
        // Get all OutLadings
        public async Task<List<OutLading>> GetOutLadingsAsync()
        {
            return await _context.OutLadings
                .Include(o => o.Truck)
                .Include(o => o.Driver)
                .Include(o => o.Order)
                .Include(o => o.FirstWeigher)
                .Include(o => o.SecondWeigher)
                .Include(o => o.User)
                .Include(o => o.TransCompany)
                .ToListAsync();
        }

        // Get OutLading by ID
        public async Task<OutLading> GetOutLadingByIdAsync(int id)
        {
            return await _context.OutLadings
                .Include(o => o.Truck)
                .Include(o => o.Driver)
                .Include(o => o.Order)
                .Include(o => o.FirstWeigher)
                .Include(o => o.SecondWeigher)
                .Include(o => o.User)
                .Include(o => o.TransCompany)
                .FirstOrDefaultAsync(o => o.LadingID == id);
        }

        // Add a new OutLading
        public async Task AddOutLadingAsync(OutLading outLading)
        {
            outLading.CreateDate = DateTime.UtcNow;
            outLading.UserID = 1;
            outLading.LadingState = "pending";
            outLading.WeightStatus = "first";


            _context.OutLadings.Add(outLading);
            await _context.SaveChangesAsync();
        }

        // Update an existing OutLading
        public async Task UpdateOutLadingAsync(OutLading outLading)
        {
            _context.OutLadings.Update(outLading);
            await _context.SaveChangesAsync();
        }

        // Delete an OutLading by ID
        public async Task DeleteOutLadingAsync(int id)
        {
            var outLading = await _context.OutLadings.FindAsync(id);
            if (outLading != null)
            {
                _context.OutLadings.Remove(outLading);
                await _context.SaveChangesAsync();
            }
        }

        // Get orders by Contract ID
        public async Task<List<Order>> GetOrdersByContractAsync(int contractId)
        {
            return await _context.Orders
                .Where(o => o.ContractID == contractId)
                .ToListAsync();
        }

        // Calculate NetWeight and Shrink for an OutLading
        public void CalculateNetWeightAndShrink(OutLading outLading)
        {
            if (outLading.WeightStatus == "FinishedSecondWeight" && outLading.FirstWeight > 0 && outLading.SecondWeight > 0)
            {
                // Calculate NetWeight using absolute value to avoid negative net weight
                outLading.NetWeight =  (double) (outLading.SecondWeight - outLading.FirstWeight);
                outLading.Shrink = outLading.Quantity - outLading.NetWeight;
                outLading.WeightStatus = "Finished"; // Update status to "Finished" once the calculation is complete
            }
        }

        // Get filtered OutLadings by status
        public async Task<List<OutLading>> GetOutLadingsByStatusAsync(string status)
        {
            return await _context.OutLadings
                .Where(o => o.LadingState == status)
                .Include(o => o.Truck)
                .Include(o => o.Driver)
                .Include(o => o.Order)
                .Include(o => o.FirstWeigher)
                .Include(o => o.SecondWeigher)
                .Include(o => o.User)
                .Include(o => o.TransCompany)
                .ToListAsync();
        }

        // Get OutLadings by Truck ID
        public async Task<List<OutLading>> GetOutLadingsByTruckAsync(int truckId)
        {
            return await _context.OutLadings
                .Where(o => o.TruckID == truckId)
                .Include(o => o.Truck)
                .Include(o => o.Driver)
                .Include(o => o.Order)
                .Include(o => o.FirstWeigher)
                .Include(o => o.SecondWeigher)
                .Include(o => o.User)
                .Include(o => o.TransCompany)
                .ToListAsync();
        }

        // Get OutLadings by Driver ID
        public async Task<List<OutLading>> GetOutLadingsByDriverAsync(int driverId)
        {
            return await _context.OutLadings
                .Where(o => o.DriverID == driverId)
                .Include(o => o.Truck)
                .Include(o => o.Driver)
                .Include(o => o.Order)
                .Include(o => o.FirstWeigher)
                .Include(o => o.SecondWeigher)
                .Include(o => o.User)
                .Include(o => o.TransCompany)
                .ToListAsync();
        }

        // Get OutLadings by Order ID
        public async Task<List<OutLading>> GetOutLadingsByOrderAsync(int orderId)
        {
            return await _context.OutLadings
                .Where(o => o.OrderID == orderId)
                .Include(o => o.Truck)
                .Include(o => o.Driver)
                .Include(o => o.Order)
                .Include(o => o.FirstWeigher)
                .Include(o => o.SecondWeigher)
                .Include(o => o.User)
                .Include(o => o.TransCompany)
                .ToListAsync();
        }
    }
}

