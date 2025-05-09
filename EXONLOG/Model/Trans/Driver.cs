﻿namespace EXONLOG.Model.Trans
{
    using EXONLOG.Model.Account;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Driver
    {
        [Key]
        public int DriverID { get; set; } // Primary Key

        [Required]
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string DriverName { get; set; } // Driver's name

        
        [Required]
        [MaxLength(20)]
        public string NationalID { get; set; } // Driver's national identification number

        [MaxLength(20)]
        public string LicenseNumber { get; set; } // Driver's license number


        [MaxLength(15)]
        [Column(TypeName = "nvarchar(100)")]
        public string? Contact { get; set; } // Driver's contact number

        [Required]
        public int UserID { get; set; } // User who created or registered the driver
        public User? User { get; set; } // Navigation property for User

        [MaxLength(250)]
        [Column(TypeName = "nvarchar(250)")]
        public string? Address { get; set; } // Driver's address

        [Column(TypeName = "nvarchar(250)")]
        public string? Notes { get; set; } // Additional notes about the driver

        public byte[]? Image { get; set; } // Optional image for the driver (stored as byte array)

        public DateTime CreateDate { get; set; } = DateTime.UtcNow; // Record creation date
    }

}
