using EXONLOG.Data;
using EXONLOG.Model.Inbound;
using Microsoft.EntityFrameworkCore;

namespace EXONLOG.Services
{
    public class ShipmentService
    {
        private readonly EXONContext _context;

        public ShipmentService(EXONContext context)
        {
            _context = context;
        }

        public async Task<List<Shipment>> GetAllShipmentsAsync()
        {
            return await _context.Shipments
                .Include(s => s.Material)
                .Include(s => s.Supplier)
                .ToListAsync();
        }

        public async Task CreateShipmentAsync(Shipment shipment)
        {
            shipment.CreateDate = DateTime.UtcNow;
           // shipment.ShipmentStatusId = "Pending";
            _context.Shipments.Add(shipment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateShipmentAsync(Shipment shipment)
        {
            _context.Entry(shipment).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteShipmentAsync(int shipmentId)
        {
            var shipment = await _context.Shipments.FindAsync(shipmentId);
            if (shipment != null)
            {
                _context.Shipments.Remove(shipment);
                await _context.SaveChangesAsync();
            }
        }
    }
}
