using EXONLOG.Model.Account;
using System.ComponentModel.DataAnnotations.Schema;

namespace EXONLOG.Model.Inbound
{

    public class Supplier
    {
        public int ID { get; set; } // Primary Key

        [Column(TypeName = "nvarchar(100)")]
        public string SupplierName { get; set; } = string.Empty; // Supplier name

        [Column(TypeName = "nvarchar(250)")]
        public string? Address { get; set; } // Supplier address

        public string? ContactNumber { get; set; } // Supplier contact number

        public string? Email { get; set; } // Supplier email

        public DateTime CreateDate { get; set; } = DateTime.Now; // Record creation date

        public int UserID { get; set; } // Foreign key to the user who created the record

        [Column(TypeName = "nvarchar(250)")]
        public string? Notes { get; set; } // Additional notes about the supplier

        // Navigation Properties
        public User? User { get; set; } // User who created the supplier
        public ICollection<Shipment>? Shipments { get; set; } // Related Incoming Shipments
    }

}
