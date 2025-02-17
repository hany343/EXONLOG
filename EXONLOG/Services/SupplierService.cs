using EXONLOG.Data;
using EXONLOG.Model.Inbound;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class SupplierService
{
    private readonly EXONContext _context;

    public SupplierService(EXONContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Fetches all suppliers from the database.
    /// </summary>
    public async Task<List<Supplier>> GetAllSuppliersAsync()
    {
        return await _context.Suppliers
            .Include(s => s.User) // Include User details
            .ToListAsync();
    }

    /// <summary>
    /// Fetches a supplier by its BatchID.
    /// </summary>
    public async Task<Supplier> GetSupplierByIdAsync(int supplierId)
    {
        return await _context.Suppliers
            .Include(s => s.User) // Include User details
            .FirstOrDefaultAsync(s => s.ID == supplierId);
    }

    /// <summary>
    /// Creates a new supplier.
    /// </summary>
    public async Task CreateSupplierAsync(Supplier supplier)
    {
        if (supplier == null)
            throw new ArgumentNullException(nameof(supplier));
        supplier.UserID = 1;
        supplier.CreateDate = DateTime.UtcNow;
        _context.Suppliers.Add(supplier);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing supplier.
    /// </summary>
    public async Task UpdateSupplierAsync(Supplier supplier)
    {
        if (supplier == null)
            throw new ArgumentNullException(nameof(supplier));

        var existingSupplier = await _context.Suppliers.FindAsync(supplier.ID);
        if (existingSupplier == null)
            throw new ArgumentException("Supplier not found.");

        existingSupplier.SupplierName = supplier.SupplierName;
        existingSupplier.Type = supplier.Type;
        existingSupplier.Address = supplier.Address;
        existingSupplier.ContactNumber = supplier.ContactNumber;
        existingSupplier.Email = supplier.Email;
        existingSupplier.Notes = supplier.Notes;

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes a supplier by its BatchID.
    /// </summary>
    public async Task DeleteSupplierAsync(int supplierId)
    {
        var supplier = await _context.Suppliers.FindAsync(supplierId);
        if (supplier == null)
            throw new ArgumentException("Supplier not found.");

        _context.Suppliers.Remove(supplier);
        await _context.SaveChangesAsync();
    }
}