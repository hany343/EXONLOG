using EXONLOG.Model.Account;
using System.ComponentModel.DataAnnotations.Schema;

namespace EXONLOG.Model.Inbound
{
    public class Batch
    {
        public int ID { get; set; } // Primary Key

        [Column(TypeName = "nvarchar(100)")]
        public string BatchNumber { get; set; } = string.Empty; // Unique reference for the batch

        public int ShipmentID { get; set; } // Foreign key to the shipment

        public double Quantity { get; set; } // Quantity in the batch

        public DateTime CreateDate { get; set; } = DateTime.Now; // Record creation date

        public int UserID { get; set; } // Foreign key to the user who created the batch

        [Column(TypeName = "nvarchar(200)")]
        public string? Notes { get; set; } // Additional notes about the batch

        // Navigation Properties
        public Shipment? Shipment { get; set; } // Related Shipment
        public User? User { get; set; } // User who created the batch
        public ICollection<InLading>? InLadings { get; set; } // Collection of related lading records
    }

}
