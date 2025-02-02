using EXONLOG.Model.Account;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EXONLOG.Model.Inbound
{
    public class Supplier
    {
        [Key]
        public int ID { get; set; } // Primary Key

        [Required(ErrorMessage = "Supplier name is required")]
        [MaxLength(150)]
        [Column(TypeName = "nvarchar(150)")]
        public string SupplierName { get; set; } = string.Empty;

        [MaxLength(250)]
        [Column(TypeName = "nvarchar(250)")]
        public string? Address { get; set; } // Supplier address

        [Phone]
        [MaxLength(15)]
        [Column(TypeName = "nvarchar(15)")]
        public string? ContactNumber { get; set; } // Supplier contact number

        [EmailAddress]
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string? Email { get; set; } // Supplier email

        [Required]
        [Column(TypeName = "datetime2(0)")]
        public DateTime CreateDate { get; set; } = DateTime.UtcNow; // Record creation date

        [Required]
        [ForeignKey("User")]
        public int UserID { get; set; } // Foreign key to the user who created the record
        public User? User { get; set; } // User who created the supplier

        [MaxLength(250)]
        [Column(TypeName = "nvarchar(250)")]
        public string? Notes { get; set; } // Additional notes about the supplier

        // Supplier Type
        public SupplierType Type { get; set; } = SupplierType.Distributor; // Default type

        // Navigation Properties
        public ICollection<Shipment>? Shipments { get; set; } // Related Incoming Shipments
    }

    public enum SupplierType
    {
        Manufacturer, // Supplier is a manufacturer
        Distributor,  // Supplier is a distributor
        Wholesaler    // Supplier is a wholesaler
    }
}