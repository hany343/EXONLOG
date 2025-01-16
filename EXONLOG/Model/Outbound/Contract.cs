namespace EXONLOG.Model.Outbound
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using EXONLOG.Model.Account;
    using EXONLOG.Model.Inbound;
    using EXONLOG.Model.Shared;

    public class Contract
    {
        [Key]
        public int ID { get; set; } // Primary Key

        
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(100)")]
        public string? RefNumber { get; set; } // Unique Reference Number for the Contract

        [Required]
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; } // Name or Title of the Contract

        [Required]
        public int UserID { get; set; } // Foreign Key to User (creator of the contract)
        public User? User { get; set; } // Navigation Property for the User who created the contract

        [Required]
        public int MaterialID { get; set; } // Foreign Key to Material
        public Material? Material { get; set; } // Navigation Property

        [Required]
        public int CustomerID { get; set; } // Foreign Key to Customer
        public Customer? Customer { get; set; } // Navigation Property for Customer

        [Required]
        [Column(TypeName = "datetime2(0)")]
        public DateTime CreateDate { get; set; } = DateTime.UtcNow; // Date the Contract was Created

        [Required]
        public double Quantity { get; set; } // Quantity of Material Required

        [Column(TypeName = "datetime2(0)")]
        public DateTime? StartDate { get; set; } // Contract Start Date

        [Column(TypeName = "datetime2(0)")]
        public DateTime? Deadline { get; set; } // Contract Deadline

        [MaxLength(500)]
        [Column(TypeName = "nvarchar(250)")]
        public string? Notes { get; set; } // Additional Notes or Comments about the Contract

        // Navigation Property for Orders
        public ICollection<Order>? Orders { get; set; } // A contract can have many orders

    }

}
