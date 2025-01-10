namespace EXONLOG.Model.Outbound
{
    using EXONLOG.Model.Account;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Order
    {
        [Key]
        public int OrderID { get; set; } // Primary Key

        [Required]
        [ForeignKey("Contract")]
        public int ContractID { get; set; } // Foreign Key to Contract
        public Contract Contract { get; set; } // Navigation Property for Contract

        [Required]
        public double Quantity { get; set; } // Quantity for the Order (as double)

        [Required]
        public DateTime OrderDate { get; set; } // Date the Order was created

        [Column(TypeName = "nvarchar(250)")]
        public string Notes { get; set; } // Additional Notes about the Order

        [Column(TypeName = "datetime2(0)")]
        public DateTime CreateDate { get; set; } = DateTime.UtcNow; // Date the Order Record was Created

        [Required]
        public int UserID { get; set; } // User who created the Order
        public User User { get; set; } // Navigation Property for User
                                       // Navigation Property for OutLadings
        public virtual ICollection<OutLading> OutLadings { get; set; } // Order has many OutLadings


    }

}
