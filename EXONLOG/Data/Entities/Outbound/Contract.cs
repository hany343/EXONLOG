namespace EXONLOG.Data.Entities.Outbound
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Contract
    {
        [Key]
        public int ID { get; set; } // Primary Key

        [Required]
        [MaxLength(50)]
        public string RefNumber { get; set; } // Unique Reference Number for the Contract

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } // Name or Title of the Contract

        [Required]
        [ForeignKey("User")]
        public int UserID { get; set; } // Foreign Key to User (creator of the contract)
        public User User { get; set; } // Navigation Property for the User who created the contract

        [Required]
        [ForeignKey("Material")]
        public int MaterialID { get; set; } // Foreign Key to Material
        public Material Material { get; set; } // Navigation Property

        [Required]
        [ForeignKey("Port")]
        public int PortID { get; set; } // Foreign Key to Port
        public Port Port { get; set; } // Navigation Property for the Port

        [Required]
        [ForeignKey("Customer")]
        public int CustomerID { get; set; } // Foreign Key to Customer
        public Customer Customer { get; set; } // Navigation Property for Customer

        [Required]
        [ForeignKey("Importer")]
        public int ImporterID { get; set; } // Foreign Key to Importer
        public Importer Importer { get; set; } // Navigation Property for Importer

        [Required]
        public DateTime CreateDate { get; set; } = DateTime.UtcNow; // Date the Contract was Created

        [Required]
        public double Quantity { get; set; } // Quantity of Material Required

        [Required]
        public DateTime StartDate { get; set; } // Contract Start Date

        [Required]
        public DateTime Deadline { get; set; } // Contract Deadline

        [MaxLength(500)]
        public string Notes { get; set; } // Additional Notes or Comments about the Contract

        // Navigation Property for Orders
        public ICollection<Order> Orders { get; set; } // A contract can have many orders

    }

}
