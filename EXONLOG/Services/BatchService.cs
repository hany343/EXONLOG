using EXONLOG.Data;
using EXONLOG.Model.Inbound;
using EXONLOG.Model.Stocks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class BatchService
{
    private readonly EXONContext _context;
    private readonly StockService _stockService;

    public BatchService(EXONContext context, StockService stockService)
    {
        _context = context;
        _stockService = stockService;
    }

    public async Task<List<Batch>> GetAllBatchesAsync()
    {
        return await _context.Batches
            .Include(b => b.Shipment)
                .ThenInclude(s => s.Material)
            .Include(b => b.User)
            .ToListAsync();
    }

    public async Task<Batch> GetBatchByIdAsync(int batchId)
    {
        return await _context.Batches
            .Include(b => b.Shipment)
            .Include(b => b.User)
            .FirstOrDefaultAsync(b => b.BatchID == batchId);
    }

    public async Task CreateBatchAsync(Batch batch)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // Get related shipment
            var shipment = await _context.Shipments
                .Include(s => s.Material)
                .FirstOrDefaultAsync(s => s.ShipmentID == batch.ShipmentID);

            if (shipment == null)
                throw new ArgumentException("Invalid Shipment ID");

            if (shipment.Quantity < batch.Quantity)
                throw new InvalidOperationException("Batch quantity exceeds shipment quantity");

            // Get raw material stock
            var stock = await _context.Stocks
                .FirstOrDefaultAsync(s => s.MaterialID == shipment.MaterialID &&
                                         s.StockType == StockType.RawMaterial);

            if (stock == null)
                throw new InvalidOperationException("Raw Material stock not found");

            // Update quantities
            shipment.Quantity -= batch.Quantity;
            stock.Quantity += batch.Quantity;
            stock.LastUpdated = DateTime.UtcNow;

            batch.CreateDate = DateTime.UtcNow;

            batch.UserID = 1;
            _context.Batches.Add(batch);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task UpdateBatchAsync(Batch batch)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var existingBatch = await _context.Batches
                .Include(b => b.Shipment)
                .FirstOrDefaultAsync(b => b.BatchID == batch.BatchID);

            if (existingBatch == null)
                throw new ArgumentException("Batch not found");

            // Handle quantity changes
            var quantityDifference = batch.Quantity - existingBatch.Quantity;

            // Get original shipment and stock
            var originalShipment = await _context.Shipments
                .Include(s => s.Material)
                .FirstOrDefaultAsync(s => s.ShipmentID == existingBatch.ShipmentID);

            var originalStock = await _context.Stocks
                .FirstOrDefaultAsync(s => s.MaterialID == originalShipment.MaterialID &&
                                         s.StockType == StockType.RawMaterial);

            // Update original values
            originalShipment.Quantity += existingBatch.Quantity;
            originalStock.Quantity -= existingBatch.Quantity;

            // Update new values
            originalShipment.Quantity -= batch.Quantity;
            originalStock.Quantity += batch.Quantity;

            // Update batch properties
            existingBatch.BatchNumber = batch.BatchNumber;
            existingBatch.Quantity = batch.Quantity;
            existingBatch.Notes = batch.Notes;
            existingBatch.ShipmentID = batch.ShipmentID;

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task DeleteBatchAsync(int batchId)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var batch = await _context.Batches
                .Include(b => b.Shipment)
                .FirstOrDefaultAsync(b => b.BatchID == batchId);

            if (batch == null) return;

            // Restore quantities
            var shipment = await _context.Shipments.FindAsync(batch.ShipmentID);
            var stock = await _context.Stocks
                .FirstOrDefaultAsync(s => s.MaterialID == shipment.MaterialID &&
                                        s.StockType == StockType.RawMaterial);

            shipment.Quantity += batch.Quantity;
            stock.Quantity -= batch.Quantity;

            _context.Batches.Remove(batch);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}