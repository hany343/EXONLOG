using EXONLOG.Data;
using EXONLOG.Model.Inbound;
using EXONLOG.Model.Stocks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ShipmentService
{
    private readonly EXONContext _context;
    private readonly StockService _stockService;

    public ShipmentService(EXONContext context, StockService stockService)
    {
        _context = context;
        _stockService = stockService;
    }

    public async Task<List<Shipment>> GetAllShipmentsAsync()
    {
        return await _context.Shipments
            .Include(s => s.Material)
            .Include(s => s.Supplier)
            .Include(s => s.Port)
            .Include(s => s.User)
            .ToListAsync();
    }

    public async Task<Shipment> GetShipmentByIdAsync(int shipmentId)
    {
        return await _context.Shipments
            .Include(s => s.Material)
            .Include(s => s.Supplier)
            .Include(s => s.Port)
            .FirstOrDefaultAsync(s => s.ShipmentID == shipmentId);
    }
    
    public async Task<List<Shipment>> GetShipmentsBySupplierIdAsync(int supplierId)
    {
        return await _context.Shipments
            .Include(s => s.Material)
            .Include(s => s.Supplier)
            .Include(s => s.Port)
            .Where(s => s.SupplierID == supplierId).ToListAsync();
    }

    public async Task CreateShipmentAsync(Shipment shipment)
    {
        if (shipment == null) throw new ArgumentNullException(nameof(shipment));

        // Add to pending quantity in stock
        var stock = await _context.Stocks
            .FirstOrDefaultAsync(s => s.MaterialID == shipment.MaterialID &&
                                    s.StockType == StockType.RawMaterial);

        if (stock == null)
            throw new InvalidOperationException("Raw Material stock not found for this material.");

        stock.PendingQuantity += shipment.Quantity;
        stock.LastUpdated = DateTime.UtcNow;

        shipment.CreateDate = DateTime.UtcNow;

        shipment.UserID = 1;
        _context.Shipments.Add(shipment);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateShipmentAsync(Shipment shipment)
    {
        var existingShipment = await _context.Shipments.FindAsync(shipment.ShipmentID);
        if (existingShipment == null) throw new ArgumentException("Shipment not found");

        // Update logic (adjust pending quantity if quantity changes)
        var originalQuantity = existingShipment.Quantity;

        if (existingShipment.Quantity != shipment.Quantity)
        {
            var stock = await _context.Stocks
                .FirstOrDefaultAsync(s => s.MaterialID == shipment.MaterialID &&
                                        s.StockType == StockType.RawMaterial);

            if (stock != null)
            {
                stock.PendingQuantity += (shipment.Quantity - originalQuantity);
                stock.LastUpdated = DateTime.UtcNow;
            }
        }

        // Update other fields
        existingShipment.ShipmentName = shipment.ShipmentName;
        existingShipment.ShipmentRef = shipment.ShipmentRef;
        existingShipment.MaterialID = shipment.MaterialID;
        existingShipment.SupplierID = shipment.SupplierID;
        existingShipment.PortID = shipment.PortID;
        existingShipment.Quantity = shipment.Quantity;
        existingShipment.ArrivalDate = shipment.ArrivalDate;
        existingShipment.Notes = shipment.Notes;
        existingShipment.ShipmentStatus = shipment.ShipmentStatus;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteShipmentAsync(int shipmentId)
    {
        var shipment = await _context.Shipments.FindAsync(shipmentId);
        if (shipment == null) throw new ArgumentException("Shipment not found");

        // Remove from pending quantity
        var stock = await _context.Stocks
            .FirstOrDefaultAsync(s => s.MaterialID == shipment.MaterialID &&
                                    s.StockType == StockType.RawMaterial);

        if (stock != null)
        {
            stock.PendingQuantity -= shipment.Quantity;
            stock.LastUpdated = DateTime.UtcNow;
        }

        _context.Shipments.Remove(shipment);
        await _context.SaveChangesAsync();
    }
}