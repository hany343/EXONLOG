using EXONLOG.Model.Account;
using EXONLOG.Model.Trans;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EXONLOG.Model.Enums;

namespace EXONLOG.Model.Inbound
{
    public class InLading
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InLadingID { get; set; }

        [Required]
        [Range(0.00, double.MaxValue, ErrorMessage = "First weight must be positive")]
        public double Quantity { get; set; } 

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


        // Enum-based status
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public WeightStatus WeightStatus { get; set; } = WeightStatus.Pending;

        // Weight validations
        [Range(0.00, double.MaxValue, ErrorMessage = "First weight must be positive")]
        public double FirstWeight { get; set; } = 0;

        [Column(TypeName = "nvarchar(150)")]
        public string? FirstWeighStation { get; set; }

        // Date validations
        [Column(TypeName = "datetime2(0)")]
        public DateTime? FirstWeighDate { get; set; }

        // Weigher relationships
        [ForeignKey("FirstWeigher")]
        public int? FirstWeigherID { get; set; }



        [Column(TypeName = "datetime2(0)")]
        public DateTime? SecondWeighDate { get; set; }

        [ForeignKey("SecondWeigher")]
        public int? SecondWeigherID { get; set; }

        [Range(0.00, double.MaxValue, ErrorMessage = "Second weight must be positive")]
        public double SecondWeight { get; set; } = 0;

        [Column(TypeName = "nvarchar(150)")]
        public string? SecondWeighStation { get; set; }

        // Calculated fields
        public double NetWeight => Math.Abs(SecondWeight - FirstWeight);

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

        [Column(TypeName = "nvarchar(250)")]
        public string? WeighNotes { get; set; } // Additional notes related to the weigh process

        // Navigation properties
        public virtual Batch? Batch { get; set; }
        public virtual Truck? Truck { get; set; }
        public virtual Driver? Driver { get; set; }
        public virtual User? User { get; set; }
        public virtual User? FirstWeigher { get; set; }
        public virtual User? SecondWeigher { get; set; }
        public virtual TransCompany? TransCompany { get; set; }

       
    }


}