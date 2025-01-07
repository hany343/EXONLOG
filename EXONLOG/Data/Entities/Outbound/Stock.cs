namespace EXONLOG.Data.Entities.Outbound
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Stock
    {
        [Key]
        public int StockID { get; set; } // Primary Key

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } // Stock Name (e.g., Warehouse A)

        [MaxLength(250)]
        public string Location { get; set; } // Physical Location of the Stock

        [Required]
        [ForeignKey("Material")]
        public int MaterialID { get; set; } // Foreign Key to Material
        public Material Material { get; set; } // Navigation Property for Material

        [Required]
        public double Quantity { get; set; } // Total Quantity of the Material in this Stock

        public string Notes { get; set; } // Additional Notes about the Stock

        public DateTime CreateDate { get; set; } = DateTime.UtcNow; // Date the Stock was Created

        [Required]
        public int UserID { get; set; } // User who created or updated the Stock
        public User User { get; set; } // Navigation Property for User
    }

}
