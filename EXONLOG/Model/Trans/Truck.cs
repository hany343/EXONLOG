namespace EXONLOG.Model.Trans
{
    using EXONLOG.Model.Account;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Truck
    {
        [Key]
        public int TruckID { get; set; } // Primary Key

        [Required]
        [MaxLength(50)]
        public string TruckNumber { get; set; } // Truck number or license plate

        [Required]
        [MaxLength(50)]
        public string LicenseNumber { get; set; } // License number for the truck

        [MaxLength(50)]
        public string? TrailerNumber { get; set; } // Trailer number (if applicable)

        [MaxLength(50)]
        public string? TrailerLicenseNumber { get; set; } // Trailer license number (if applicable)

        [Required]
        [MaxLength(100)]
        public string OwnerName { get; set; } // Owner's name

        [MaxLength(50)]
        public string? TruckType { get; set; } // Type of truck (e.g., "Flatbed", "Tanker")

        public double? Capacity { get; set; } // Maximum load capacity of the truck

        public string? Notes { get; set; } // Additional notes about the truck

        public DateTime CreateDate { get; set; } = DateTime.UtcNow; // Record creation date

        [Required]
        public int UserID { get; set; } // Foreign Key to the User who created the material
        public User? User { get; set; } // Navigation Property for User
    }

}
