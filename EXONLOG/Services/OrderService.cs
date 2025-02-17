namespace EXONLOG.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using EXONLOG.Data;
    using EXONLOG.Model.Outbound;
    using Microsoft.EntityFrameworkCore;

   
public class OrderService
    {
        private readonly EXONContext _dbContext;

        public OrderService(EXONContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Retrieves all orders from the database.
        /// </summary>
        /// <returns>List of orders</returns>
        public Task<List<Order>> GetAllOrdersAsync()
        {
            try
            {
                return _dbContext.Orders
                    .Include(o => o.Contract!).ThenInclude(c => c.Material!) // Eager load the related Contract
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework like Serilog, NLog, etc.)
                Console.WriteLine($"An error occurred: {ex.Message}");

                // Optionally, rethrow the exception or return a default value
                throw;
            }
        }

        /// <summary>
        /// Retrieves a single order by its BatchID.
        /// </summary>
        /// <param name="orderId">Order BatchID</param>
        /// <returns>Order</returns>
        public Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return _dbContext.Orders
                .Include(o => o.Contract!) // Include related Contract
                .FirstOrDefaultAsync(o => o.OrderID == orderId);
        }

        /// <summary>
        /// Adds a new order to the database.
        /// </summary>
        /// <param name="order">Order to add</param>
        /// <returns>Task</returns>
        public async Task AddOrderAsync(Order order)
        {
            var contract = await _dbContext.Contracts
                .Include(c => c.Orders!) // Include existing orders
                .FirstOrDefaultAsync(c => c.ContractID == order.ContractID);

            if (contract == null)
                throw new Exception("Contract not found.");

            double totalOrderedQuantity = contract.Orders.Sum(o => o.Quantity);
            if (totalOrderedQuantity + order.Quantity > contract.Quantity)
                throw new Exception("Order quantity exceeds the remaining contract quantity.");
            order.UserID = 1;
            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing order in the database.
        /// </summary>
        /// <param name="order">Order to update</param>
        /// <returns>Task</returns>
        public async Task UpdateOrderAsync(Order order)
        {
            var existingOrder = await _dbContext.Orders.FindAsync(order.OrderID);
            if (existingOrder == null)
                throw new Exception("Order not found.");

            var contract = await _dbContext.Contracts
                .Include(c => c.Orders!) // Include existing orders
                .FirstOrDefaultAsync(c => c.ContractID == order.ContractID);

            if (contract == null)
                throw new Exception("Contract not found.");

            // Check if updating the order quantity would exceed the remaining contract quantity
            double totalOrderedQuantity = contract.Orders
                .Where(o => o.OrderID != order.OrderID) // Exclude the current order being updated
                .Sum(o => o.Quantity);

            if (totalOrderedQuantity + order.Quantity > contract.Quantity)
                throw new Exception("Updated order quantity exceeds the remaining contract quantity.");

            // Update the order
            existingOrder.OrderNumber = order.OrderNumber;
            existingOrder.Quantity = order.Quantity;
            existingOrder.CreateDate = order.CreateDate;
            existingOrder.ContractID = order.ContractID;

            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes an order from the database.
        /// </summary>
        /// <param name="orderId">Order BatchID</param>
        /// <returns>Task</returns>
        public async Task DeleteOrderAsync(int orderId)
        {
            var order = await _dbContext.Orders.FindAsync(orderId);
            if (order == null)
                throw new Exception("Order not found.");

            _dbContext.Orders.Remove(order);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Order>> GetOrdersByContractIdAsync(int contractId)
        {
            return await _dbContext.Orders.Where(o => o.ContractID == contractId).ToListAsync();
        }

        public List<Order> GetOrdersByContractId(int contractId)
        {
            return _dbContext.Orders.Where(o => o.ContractID == contractId).ToList();
        }
    }

}


