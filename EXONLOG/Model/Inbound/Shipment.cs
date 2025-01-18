using EXONLOG.Model.Account;
using EXONLOG.Model.Outbound;
using EXONLOG.Model.Stocks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EXONLOG.Model.Inbound
{
    public class Shipment
    {
        public int ID { get; set; } // Primary Key

        [Column(TypeName = "nvarchar(100)")]
        public string ShipmentRef { get; set; } = string.Empty; // Unique reference for the shipment


        [Column(TypeName = "nvarchar(100)")]
        public string ShipmentName { get; set; } = string.Empty; // Unique reference for the shipment

        public int MaterialID { get; set; } // Foreign key to the material

        public int PortID { get; set; } // Foreign key to the port

        public int SupplierID { get; set; } // Foreign key to the supplier

        public double Quantity { get; set; } // Total quantity in the shipment

        public DateTime ArrivalDate { get; set; } // Date the shipment arrives

        [Column(TypeName = "nvarchar(250)")]
        public string? Notes { get; set; } // Additional notes about the shipment

        public DateTime CreateDate { get; set; } = DateTime.Now; // Record creation date

        public int UserID { get; set; } // Foreign key to the user who registered the shipment

        // Navigation Properties
        public int ShipmentStatusId { get; set; }
        public ShipmentStatus? ShipmentStatus { get; set; }
        public Material? Material { get; set; } // Related Material
        public Port? Port { get; set; } // Related Port
        public Supplier? Supplier { get; set; } // Related Supplier
        public User? User { get; set; } // User who registered the shipment
        public ICollection<Batch>? Batches { get; set; } // Collection of related batches

        [Required]
        [ForeignKey("Importer")]
        public int ImporterID { get; set; } // Foreign Key to Importer
        public Importer? Importer { get; set; } // Navigation Property for Importer

    }

}
