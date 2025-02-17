using EXONLOG.Data;
using EXONLOG.Model.Stocks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class StockService
{
    private readonly EXONContext _context;

    public StockService(EXONContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Fetches all stocks from the database.
    /// </summary>
    public async Task<List<Stock>> GetAllStocksAsync()
    {
        return await _context.Stocks
            .Include(s => s.Material) // Include Material details
            .ToListAsync();
    }

    /// <summary>
    /// Fetches a stock by its ID.
    /// </summary>
    public async Task<Stock> GetStockByIdAsync(int stockId)
    {
        return await _context.Stocks
            .Include(s => s.Material) // Include Material details
            .FirstOrDefaultAsync(s => s.StockID == stockId);
    }

    /// <summary>
    /// Creates a new stock entry.
    /// </summary>
    public async Task CreateStockAsync(Stock stock)
    {
        if (stock == null)
            throw new ArgumentNullException(nameof(stock));

        // Validate material
        var material = await _context.Materials.FindAsync(stock.MaterialID);
        if (material == null)
            throw new ArgumentException("Invalid Material ID.");

        // Set default values
        stock.CreateDate = DateTime.UtcNow;
        stock.LastUpdated = DateTime.UtcNow;

        stock.UserID = 1;
        _context.Stocks.Add(stock);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing stock entry.
    /// </summary>
    public async Task UpdateStockAsync(Stock stock)
    {
        if (stock == null)
            throw new ArgumentNullException(nameof(stock));

        var existingStock = await _context.Stocks.FindAsync(stock.StockID);
        if (existingStock == null)
            throw new ArgumentException("Stock not found.");

        // Update properties
        existingStock.StockName = stock.StockName;
        existingStock.MaterialID = stock.MaterialID;
        existingStock.StockType = stock.StockType;
        existingStock.Quantity = stock.Quantity;
        existingStock.ReservedQuantity = stock.ReservedQuantity;
        existingStock.PendingQuantity = stock.PendingQuantity;
        existingStock.Notes = stock.Notes;
        existingStock.LastUpdated = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes a stock entry by its ID.
    /// </summary>
    public async Task DeleteStockAsync(int stockId)
    {
        var stock = await _context.Stocks.FindAsync(stockId);
        if (stock == null)
            throw new ArgumentException("Stock not found.");

        _context.Stocks.Remove(stock);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates stock quantity when receiving InLading (for Raw Material stock).
    /// </summary>
    public async Task UpdateStockForInLadingAsync(int stockId, double netWeight)
    {
        var stock = await _context.Stocks.FindAsync(stockId);
        if (stock == null)
            throw new ArgumentException("Stock not found.");

        if (stock.StockType != StockType.RawMaterial)
            throw new InvalidOperationException("Only Raw Material stock can be updated with InLading.");

        stock.Quantity += netWeight;
        stock.LastUpdated = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates stock quantity when creating OutLading (for Outbound stock).
    /// </summary>
    public async Task UpdateStockForOutLadingAsync(int stockId, double netWeight)
    {
        var stock = await _context.Stocks.FindAsync(stockId);
        if (stock == null)
            throw new ArgumentException("Stock not found.");

        if (stock.StockType != StockType.Product)
            throw new InvalidOperationException("Only Outbound stock can be updated with OutLading.");

        if (stock.Quantity < netWeight)
            throw new InvalidOperationException("Insufficient stock quantity.");

        stock.Quantity -= netWeight;
        stock.ReservedQuantity -= netWeight;
        stock.LastUpdated = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Reserves stock quantity for a contract (for Outbound stock).
    /// </summary>
    public async Task ReserveStockForContractAsync(int stockId, double quantity)
    {
        var stock = await _context.Stocks.FindAsync(stockId);
        if (stock == null)
            throw new ArgumentException("Stock not found.");

        if (stock.StockType != StockType.Product)
            throw new InvalidOperationException("Only Outbound stock can be reserved for contracts.");

        if (stock.FreeQuantity < quantity)
            throw new InvalidOperationException("Insufficient free stock quantity.");

        stock.ReservedQuantity += quantity;
        stock.LastUpdated = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Releases reserved stock quantity for a contract (for Outbound stock).
    /// </summary>
    public async Task ReleaseReservedStockAsync(int stockId, double quantity)
    {
        var stock = await _context.Stocks.FindAsync(stockId);
        if (stock == null)
            throw new ArgumentException("Stock not found.");

        if (stock.StockType != StockType.Product)
            throw new InvalidOperationException("Only Outbound stock can release reserved quantity.");

        if (stock.ReservedQuantity < quantity)
            throw new InvalidOperationException("Insufficient reserved stock quantity.");

        stock.ReservedQuantity -= quantity;
        stock.LastUpdated = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }
}