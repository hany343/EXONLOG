using EXONLOG.Data;
using EXONLOG.Model.Outbound;
using Microsoft.EntityFrameworkCore;
using System;
using EXONLOG.Model.Enums;
namespace EXONLOG.Services
{
    public class OutLadingService
    {
        private readonly EXONContext _context;

        public OutLadingService(EXONContext context)
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

        // Get OutLading by BatchID
        public OutLading GetOutLadingByIdAsync(int id)
        {
            return  _context.OutLadings
                .Include(o => o.Truck)
                .Include(o => o.Driver)
                .Include(o => o.Order)
                .Include(o => o.FirstWeigher)
                .Include(o => o.SecondWeigher)
                .Include(o => o.User)
                .Include(o => o.TransCompany)
                .FirstOrDefault(o => o.OutLadingID == id);
        }

        /// <summary>
        /// Fetches all OutLadings associated with a specific OrderID.
        /// </summary>
        /// <param name="orderId">The BatchID of the order.</param>
        /// <returns>A list of OutLadings with related data.</returns>
        public async Task<List<OutLading>> GetOutLadingsByOrderIdAsync(int orderId)
        {
            return await _context.OutLadings
                .Include(ol => ol.Truck) // Include Truck details
                .Include(ol => ol.Driver) // Include Driver details
                .Include(ol => ol.TransCompany) // Include TransCompany details
                .Where(ol => ol.OrderID == orderId) // Filter by OrderID
                .ToListAsync();
        }
        // Add a new OutLading
        public async Task AddOutLadingAsync(OutLading outLading)
        {
            outLading.CreateDate = DateTime.UtcNow;
            outLading.UserID = 1;
            outLading.LadingState = "pending";
            outLading.WeightStatus = WeightStatus.Pending;


            _context.OutLadings.Add(outLading);
            await _context.SaveChangesAsync();
        }
        public async Task<(bool Success, string Message, OutLading? SavedOutLading)> CreateOutLadingAsync(OutLading outLading)
        {
            if (outLading.OrderID <= 0)
                return (false, "Invalid OrderID", null);

            var order = await _context.Orders
                .Include(o => o.OutLadings)
                .FirstOrDefaultAsync(o => o.OrderID == outLading.OrderID);

            if (order == null)
                return (false, "Invalid Order", null);

            var totalUsed = order.OutLadings?.Sum(ol => ol.Quantity) ?? 0;

            if (outLading.Quantity > (order.Quantity - totalUsed))
                return (false, "Quantity exceeds available order quantity", null);

            outLading.UserID = GetCurrentUserId(); // Replace with actual user retrieval logic

            _context.OutLadings.Add(outLading);

            try
            {
                await _context.SaveChangesAsync();
                return (true, "OutLading saved successfully", outLading);
            }
            catch (Exception ex)
            {
                return (false, $"Database error: {ex.Message}", null);
            }
        }

        private int GetCurrentUserId()
        {
            return 1; // Replace with real user BatchID retrieval logic
        }


        public async Task<List<OutLading>> GetAllOutLadingsAsync()
        {
            return await _context.OutLadings
                .Include(ol => ol.Order)
                    .ThenInclude(o => o.Contract)
                .Include(ol => ol.Truck)
                .Include(ol => ol.Driver)
                .ToListAsync();
        }
        // Update an existing OutLading
        public async Task UpdateOutLadingAsync(OutLading outLading)
        {
            _context.OutLadings.Update(outLading);
            await _context.SaveChangesAsync();
        }

        // Delete an OutLading by BatchID
        public async Task DeleteOutLadingAsync(int id)
        {
            var outLading = await _context.OutLadings.FindAsync(id);
            if (outLading != null)
            {
                _context.OutLadings.Remove(outLading);
                await _context.SaveChangesAsync();
            }
        }

        // Get orders by Contract BatchID
        public async Task<List<Order>> GetOrdersByContractAsync(int contractId)
        {
            return await _context.Orders
                .Where(o => o.ContractID == contractId)
                .ToListAsync();
        }

        // Calculate NetWeight and Shrink for an OutLading
        //public void CalculateNetWeightAndShrink(OutLading outLading)
        //{
        //    if (outLading.WeightStatus == WeightStatus.SecondWeighCompleted && outLading.FirstWeight > 0 && outLading.SecondWeight > 0)
        //    {
        //        // Calculate NetWeight using absolute value to avoid negative net weight
        //        outLading.NetWeight =  (double) (outLading.SecondWeight - outLading.FirstWeight);
        //        outLading.Shrink = outLading.Quantity - outLading.NetWeight;
        //        outLading.WeightStatus = "Finished"; // Update status to "Finished" once the calculation is complete
        //    }
        //}

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

        // Get OutLadings by Truck BatchID
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

        // Get OutLadings by Driver BatchID
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

        // Get OutLadings by Order BatchID
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

