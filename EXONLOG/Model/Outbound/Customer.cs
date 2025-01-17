namespace EXONLOG.Model.Outbound
{
    using EXONLOG.Model.Account;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Customer
    {
        [Key]
        public int CustomerID { get; set; } // Primary Key

        [Required]
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string CustomerName { get; set; } // Customer MaterialName

        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string? Address { get; set; } // Customer Address

        [Phone]
        [Column(TypeName = "nvarchar(100)")]
        public string? ContactNumber { get; set; } // Customer's Contact Number

        [EmailAddress]
        [Column(TypeName = "nvarchar(100)")]
        public string? Email { get; set; } // Customer's Email Address

        [Column(TypeName = "nvarchar(250)")]
        public string? Notes { get; set; } // Additional Notes
        
        [Column(TypeName = "datetime2(0)")]
        public DateTime CreateDate { get; set; } = DateTime.UtcNow; // Date the Customer was Created

        [Required]
        public int UserID { get; set; } // Foreign Key to the User who created the customer
        public User? User { get; set; } // Navigation Property for User

        public ICollection<Contract>? Contracts { get; set; } // A customer can have many contracts
    }

}
