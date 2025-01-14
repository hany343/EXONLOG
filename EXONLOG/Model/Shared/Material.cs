namespace EXONLOG.Model.Shared
{

    using EXONLOG.Model.Account;
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
        public string Name { get; set; } // Material Name

        [MaxLength(500)]
        [Column(TypeName = "nvarchar(100)")]
        public string? Description { get; set; } // Description of the Material

        [Column(TypeName = "nvarchar(200)")]
        public string? Notes { get; set; } // Additional Notes

        public DateTime CreateDate { get; set; } = DateTime.UtcNow; // Date the Material was Created

        [Required]
        public int UserID { get; set; } // Foreign Key to the User who created the material
        public User? User { get; set; } // Navigation Property for User


        // One-to-many relationship with Stock
        public ICollection<Stock> Stocks { get; set; } = new List<Stock>(); // A material can have multiple stock entries

        // Computed Property for Available Quantity (from all Stock entries)
        [NotMapped]
        public double AvailableQuantity
        {
            get
            {
                // Sum the quantity of all related stock entries
                return Stocks?.Sum(s => s.Quantity) ?? 0;
            }
        }

        //public double GetAvailableQuantity(int materialId)
        //{
        //    var material = dbContext.Materials
        //                            .Include(m => m.Stocks) // Ensure related Stocks are loaded
        //                            .FirstOrDefault(m => m.MaterialID == materialId);

        //    return material?.AvailableQuantity ?? 0; // Get available quantity or 0 if material not found
        //}
    }

}
