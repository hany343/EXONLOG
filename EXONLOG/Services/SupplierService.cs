using EXONLOG.Data;
using EXONLOG.Model.Inbound;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace EXONLOG.Services
{
    public class SupplierService
    {
        private readonly EXONContext _context;

        public SupplierService(EXONContext context)
        {
            _context = context;
        }

        public async Task<List<Supplier>> GetSuppliersAsync(string searchTerm = "")
        {
            return await _context.Suppliers
                .Where(s => string.IsNullOrEmpty(searchTerm) || s.SupplierName.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<Supplier?> GetSupplierByIdAsync(int id)
        {
            return await _context.Suppliers.FindAsync(id);
        }

        public async Task AddSupplierAsync(Supplier supplier)
        {
            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSupplierAsync(Supplier supplier)
        {
            _context.Suppliers.Update(supplier);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSupplierAsync(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier != null)
            {
                _context.Suppliers.Remove(supplier);
                await _context.SaveChangesAsync();
            }
        }
        //public async Task<byte[]> ExportSuppliersToCsv()
        //{
        //    var suppliers = await _context.Suppliers.ToListAsync();
        //    using var memoryStream = new MemoryStream();
        //    using var streamWriter = new StreamWriter(memoryStream);
        //    using var csvWriter = new CsvWriter(streamWriter);

        //    csvWriter.SetColumns(new[] { "SupplierId", "SupplierName", "ContactName", "City", "Country" });
        //    foreach (var supplier in suppliers)
        //    {
        //        csvWriter.WriteValueStart();
        //        csvWriter.Write(supplier.ID.ToString());
        //        csvWriter.WriteCellDelimiter();
        //        csvWriter.Write(supplier.SupplierName);
        //        csvWriter.WriteCellDelimiter();
        //        csvWriter.Write(supplier.ContactName);
        //        csvWriter.WriteCellDelimiter();
        //        csvWriter.Write(supplier.City);
        //        csvWriter.WriteCellDelimiter();
        //        csvWriter.Write(supplier.Country);
        //        csvWriter.WriteValueEnd();
        //        csvWriter.NextRow();
        //    }

        //    await streamWriter.FlushAsync();
        //    return memoryStream.ToArray();
        //}
    }
}
