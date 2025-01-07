namespace EXONLOG.Data.Trans
{
    using EXONLOG.Data.Entities;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Driver
    {
        [Key]
        public int DriverID { get; set; } // Primary Key

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } // Driver's name

        [MaxLength(15)]
        public string Contact { get; set; } // Driver's contact number

        [MaxLength(20)]
        public string LicenseNumber { get; set; } // Driver's license number

        [Required]
        [MaxLength(20)]
        public string NationalID { get; set; } // Driver's national identification number

        [Required]
        public int UserID { get; set; } // User who created or registered the driver
        public User User { get; set; } // Navigation property for User

        [MaxLength(250)]
        public string Address { get; set; } // Driver's address

        public string Notes { get; set; } // Additional notes about the driver

        public byte[]? Image { get; set; } // Optional image for the driver (stored as byte array)

        public DateTime CreateDate { get; set; } = DateTime.UtcNow; // Record creation date
    }

}
