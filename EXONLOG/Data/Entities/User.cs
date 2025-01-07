namespace EXONLOG.Data.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using EXONLOG.Data.Entities.Outbound;

    public class User
    {
        [Key]
        public int UserID { get; set; } // Primary Key

        [Required]
        [MaxLength(50)]
        public string Username { get; set; } // Unique Username

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } // Full Name of the User

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } // Email Address

        [Required]
        [MaxLength(15)]
        [Phone]
        public string MobileNumber { get; set; } // Mobile Number

        [Required]
        [MaxLength(256)]
        public string PasswordHash { get; set; } // Hashed Password

        [Required]
        public bool IsActive { get; set; } = true; // Indicates if the user is active

        [MaxLength(20)]
        public string Role { get; set; } // Role of the user (e.g., "Admin", "StockManager", etc.)

        public string Notes { get; set; } // Additional Notes about the User

        public DateTime CreateDate { get; set; } = DateTime.UtcNow; // Date the User was Created

        // Navigation Properties
        public ICollection<Material> Materials { get; set; } // Materials created by the User
        public ICollection<Customer> Customers { get; set; } // Customers created by the User
        public ICollection<Importer> Importers { get; set; } // Importers created by the User
        public ICollection<Port> Ports { get; set; } // Ports created by the User
        public ICollection<Contract> Contracts { get; set; } // Contracts created by the User
    }

}
