namespace EXONLOG.Model.Shared
{
    using EXONLOG.Model.Account;
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
        public string Description { get; set; } // Description of the Material

        public double QuantityInStock { get; set; } // Quantity in Stock

        [Column(TypeName = "nvarchar(200)")]
        public string Notes { get; set; } // Additional Notes

        public DateTime CreateDate { get; set; } = DateTime.UtcNow; // Date the Material was Created

        [Required]
        public int UserID { get; set; } // Foreign Key to the User who created the material
        public User User { get; set; } // Navigation Property for User

        // Navigation Property for Stock
        public Stock Stock { get; set; } // One-to-One Relationship with Stock

    }

}
