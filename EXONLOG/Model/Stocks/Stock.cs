using EXONLOG.Model.Account;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EXONLOG.Model.Stocks
{
    public class Stock
    {
        [Key]
        public int StockID { get; set; }

        [Required]
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string StockName { get; set; } // Stock name (e.g., "Warehouse A")

        [Required]
        public int MaterialID { get; set; } // Foreign Key to Material
        public Material? Material { get; set; } // Navigation Property

        [Required]
        public double Quantity { get; set; } // Total quantity in stock

        public double ReservedQuantity { get; set; } = 0; // Quantity reserved for contracts

        public double PendingQuantity { get; set; } = 0; // Quantity pending from shipments

        [Required]
        public StockType StockType { get; set; } // Raw Material or Outbound

        [Column(TypeName = "nvarchar(200)")]
        public string? Notes { get; set; } // Additional notes

        public DateTime CreateDate { get; set; } = DateTime.UtcNow; // Record creation date

        [Column(TypeName = "datetime2(0)")]
        public DateTime? LastUpdated { get; set; } // Last time stock was updated

        [Required]
        public int UserID { get; set; } // User who created or updated the stock
        public User? User { get; set; } // Navigation Property

        // Computed Properties
        [NotMapped]
        public double FreeQuantity => Quantity - ReservedQuantity; // Available quantity

        [NotMapped]
        public double TotalAvailableQuantity => FreeQuantity + PendingQuantity; // Total available (including pending)
    }

    public enum StockType
    {
        RawMaterial, // Stock for raw materials (starts at 0, increases with InLading)
        Product     // Stock for Product materials (starts with quantity, decreases with OutLading)
    }
}