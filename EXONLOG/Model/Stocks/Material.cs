namespace EXONLOG.Model.Stocks
{

    using EXONLOG.Model.Account;
    using EXONLOG.Model.Outbound;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Material
    {

        [Key]
        public int MaterialID { get; set; } // Primary Key

        [Required]
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string MaterialName { get; set; } // Material MaterialName

        [Required]
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string Prefix { get; set; } // Material MaterialName


        [MaxLength(500)]
        [Column(TypeName = "nvarchar(100)")]
        public string? Description { get; set; } // Description of the Material

        [Column(TypeName = "nvarchar(200)")]
        public string? Notes { get; set; } // Additional Notes

        public DateTime CreateDate { get; set; } = DateTime.UtcNow; // Date the Material was Created

        [Required]
        public int UserID { get; set; } // Foreign Key to the User who created the material
        public User? User { get; set; } // Navigation Property for User

        [Required]
        public int MaterialTypeId { get; set; } // Foreign Key to the User who created the material
        public MaterialType? MaterialType { get; set; } // Navigation Property for User

        // One-to-many relationship with Stock
        public ICollection<Stock>? Stocks { get; set; } = new List<Stock>(); // A material can have multiple stock entries
        public ICollection<Contract>? Contracts { get; set; } = new List<Contract>(); // A material can have multiple stock entries

        // Computed Property for Available Quantity (from all Stock entries)

        //public double FreeQuantityInStock { get; set; }

        // Dynamically calculate the total stock quantity

        // These properties will be computed by SQL Server
        [NotMapped]
        public double ReservedQuantity { get; set; }
        [NotMapped]
        public double TotalStockQuantity { get; set; }
        [NotMapped]
        public double FreeQuantityInStock { get; set; }

           // public double ReservedQuantity => Contracts?.Sum(contract => contract.Quantity) ?? 0; // Reserved Quantity
           // public double TotalStockQuantity => Stocks?.Sum(stock => stock.Quantity) ?? 0;

           //// Dynamically calculate the available quantity
           //public double FreeQuantityInStock => TotalStockQuantity - ReservedQuantity;
        //}

       }


    }


