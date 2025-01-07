namespace EXONLOG.Data.Entities.Outbound
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Importer
    {
        [Key]
        public int ImporterID { get; set; } // Primary Key

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } // Importer Name

        [MaxLength(250)]
        public string Address { get; set; } // Importer's Address

        [Phone]
        public string ContactNumber { get; set; } // Importer's Contact Number

        [EmailAddress]
        public string Email { get; set; } // Importer's Email Address

        public string Notes { get; set; } // Additional Notes

        public DateTime CreateDate { get; set; } = DateTime.UtcNow; // Date the Importer was Created

        [Required]
        public int UserID { get; set; } // Foreign Key to the User who created the importer
        public User User { get; set; } // Navigation Property for User
    }

}
