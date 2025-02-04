using EXONLOG.Model.Account;
using EXONLOG.Model.Trans;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EXONLOG.Model.Inbound
{
    public class InLading
    {
        [Key]
        public int InladID { get; set; }

        //// Unique lading reference with validation
        //[Required(ErrorMessage = "Lading reference is required")]
        //[MaxLength(100, ErrorMessage = "Lading reference cannot exceed 100 characters")]
        //[Column(TypeName = "nvarchar(100)")]
        //[Index(IsUnique = true)] // Ensure unique reference
        //public string LadingRef { get; set; } = string.Empty;

        // Foreign keys with explicit relationships
        [Required(ErrorMessage = "Batch is required")]
        [ForeignKey("Batch")]
        public int BatchID { get; set; }

        [Required(ErrorMessage = "Truck is required")]
        [ForeignKey("Truck")]
        public int TruckID { get; set; }

        [Required(ErrorMessage = "Driver is required")]
        [ForeignKey("Driver")]
        public int DriverID { get; set; }

        // Weight validations
        [Range(0.00, double.MaxValue, ErrorMessage = "First weight must be positive")]
        public double FirstWeight { get; set; } = 0;

        [Range(0.00, double.MaxValue, ErrorMessage = "Second weight must be positive")]
        public double SecondWeight { get; set; } = 0;

        // Computed net weight (not stored in DB)
        [NotMapped]
        public double NetWeight => Math.Abs(SecondWeight - FirstWeight);

        // Date validations
        [Column(TypeName = "datetime2(0)")]
        public DateTime? FirstWeighDate { get; set; }


        [Column(TypeName = "datetime2(0)")]
        public DateTime? SecondWeighDate { get; set; }

        // Enum-based status
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public WeightStatus WeightStatus { get; set; } = WeightStatus.Pending;

        // Weigher relationships
        [ForeignKey("FirstWeigher")]
        public int? FirstWeigherID { get; set; }


        [ForeignKey("SecondWeigher")]
        public int? SecondWeigherID { get; set; }

        // Additional fields
        [MaxLength(250, ErrorMessage = "Notes cannot exceed 250 characters")]
        [Column(TypeName = "nvarchar(250)")]
        public string? Notes { get; set; }

        [Required]
        [Column(TypeName = "datetime2(0)")]
        public DateTime CreateDate { get; set; } = DateTime.UtcNow; // Use UTC

        // User relationships
        [Required(ErrorMessage = "User is required")]
        [ForeignKey("User")]
        public int UserID { get; set; }

        // Transportation company relationship
        [Required(ErrorMessage = "Transport company is required")]
        [ForeignKey("TransCompany")]
        public int TransCompanyID { get; set; }

        // Navigation properties
        public virtual Batch? Batch { get; set; }
        public virtual Truck? Truck { get; set; }
        public virtual Driver? Driver { get; set; }
        public virtual User? User { get; set; }
        public virtual User? FirstWeigher { get; set; }
        public virtual User? SecondWeigher { get; set; }
        public virtual TransCompany? TransCompany { get; set; }

       
    }

    public enum WeightStatus
    {
        Pending,
        FirstWeighCompleted,
        SecondWeighCompleted,
        Verified,
        Disputed
    }
}