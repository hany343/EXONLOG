namespace EXONLOG.Model.Shared
{
    using EXONLOG.Model.Account;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Stock
    {
        [Key]
        public int StockID { get; set; } // Primary Key

        [Required]
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; } // Stock Name (e.g., Warehouse A)
        

        [MaxLength(250)]
        [Column(TypeName = "nvarchar(100)")]
        public string? Location { get; set; } // Physical Location of the Stock

        [Required]
        [ForeignKey("Material")]
        public int MaterialID { get; set; } // Foreign Key to Material
        public Material Material { get; set; } // Navigation Property for Material

        [Required]
        public double Quantity { get; set; } // Total Quantity of the Material in this Stock

        [Column(TypeName = "nvarchar(200)")]
        public string? Notes { get; set; } // Additional Notes about the Stock

        public DateTime CreateDate { get; set; } = DateTime.UtcNow; // Date the Stock was Created
        public DateTime? LastUpdated { get; set; } // Last time stock was updated


        [Required]
        public int UserID { get; set; } // User who created or updated the Stock
        public User? User { get; set; } // Navigation Property for User
    }

}
