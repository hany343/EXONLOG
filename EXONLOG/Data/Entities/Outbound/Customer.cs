namespace EXONLOG.Data.Entities.Outbound
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Customer
    {
        [Key]
        public int CustomerID { get; set; } // Primary Key

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } // Customer Name

        [MaxLength(250)]
        public string Address { get; set; } // Customer Address

        [Phone]
        public string ContactNumber { get; set; } // Customer's Contact Number

        [EmailAddress]
        public string Email { get; set; } // Customer's Email Address

        public string Notes { get; set; } // Additional Notes

        public DateTime CreateDate { get; set; } = DateTime.UtcNow; // Date the Customer was Created

        [Required]
        public int UserID { get; set; } // Foreign Key to the User who created the customer
        public User User { get; set; } // Navigation Property for User

        public ICollection<Contract>? Contracts { get; set; } // A customer can have many contracts
    }

}
