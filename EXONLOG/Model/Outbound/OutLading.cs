using EXONLOG.Model.Account;
using EXONLOG.Model.Inbound;
using EXONLOG.Model.Trans;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EXONLOG.Model.Enums;

namespace EXONLOG.Model.Outbound
{
    public class OutLading
    {
        [Key]
        public int OutLadingID { get; set; }


        [Required]
        [Range(0.00, double.MaxValue, ErrorMessage = "First weight must be positive")]
        public double Quantity { get; set; }

        // Foreign keys with explicit relationships
        [Required(ErrorMessage = "Order is required")]
        [ForeignKey("Order")]
        public int OrderID { get; set; } // Foreign Key to Order

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



        [Column(TypeName = "nvarchar(100)")]
        public  string? FillType { get; set; } // Fill type: full, half, etc.
        [Column(TypeName = "nvarchar(100)")]
        public string? StackBar { get; set; } // Stack bar identifier
        public int? BagsCount { get; set; } // Number of bags in the shipment

        [Column(TypeName = "nvarchar(100)")]
        public string? ShippingAddress { get; set; } // Shipping address for delivery

        [Column(TypeName = "nvarchar(100)")]
        public string? ShippingCity { get; set; } // City of the shipping address

        [Column(TypeName = "datetime2(0)")]
        public DateTime? LeaveDate { get; set; } // Date when goods leave
        public int? ApprovedUserID { get; set; } // User who approves the lading

        [Column(TypeName = "nvarchar(100)")]
        public string? RepresentativeName { get; set; } // MaterialName of the representative for the shipment
        [Column(TypeName = "nvarchar(250)")]
        public string? WeighNotes { get; set; } // Additional notes related to the weigh process

        
        [Column(TypeName = "nvarchar(100)")]
        public  string? LadingState { get; set; } // State of the lading: "In Progress", "Completed"

       
        [Column(TypeName = "nvarchar(100)")]
        // New ShippingType column as Enum
        public  string ShippingType { get; set; } // Type of shipping: Air, Sea, Road, etc.


       
        public TransCompany? TransCompany { get; set; }  // Navigation property
        // Navigation properties
        public virtual User? FirstWeigher { get; set; }
        public virtual User? SecondWeigher { get; set; }
        public virtual User? User { get; set; } // The user who created the OutLading
        public virtual Truck? Truck { get; set; }
        public virtual Driver? Driver { get; set; }
        public virtual Order? Order { get; set; } // Reference to the related Order

        
    }
    
}
