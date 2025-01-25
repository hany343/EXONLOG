using EXONLOG.Model.Account;
using EXONLOG.Model.Trans;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EXONLOG.Model.Outbound
{
    public class OutLading
    {
        [Key]
        public int LadingID { get; set; }

       
        public int TruckID { get; set; } // Foreign Key to Truck

        
        public int DriverID { get; set; } // Foreign Key to Driver

        
        public int OrderID { get; set; } // Foreign Key to Order

       
        [Column(TypeName = "nvarchar(100)")]
        public string? Type { get; set; } // "Inbound" or "Outbound"

        // First Weight Information
        public double? FirstWeight { get; set; } // Weight after first weighbridge
        public int? FirstWeigherID { get; set; } // Foreign Key to User who performed the first weigh

        [Column(TypeName = "datetime2(0)")]
        public DateTime? FirstWeightDate { get; set; } // Date and time of first weighing

        // Second Weight Information
        public double? SecondWeight { get; set; } // Weight after second weighbridge
        public int? SecondWeigherID { get; set; } // Foreign Key to User who performed the second weigh
        [Column(TypeName = "datetime2(0)")]
        public DateTime? SecondWeightDate { get; set; } // Date and time of second weighing

        // Net Weight Information
        public double NetWeight { get; set; } // Net weight (auto-calculated)
        public double Shrink { get; set; } // Shrinkage between quantity and net weight (auto-calculated)

        // Additional Fields
        public double Quantity { get; set; } // Quantity of materials

        
        [Column(TypeName = "nvarchar(100)")]
        public  string FillType { get; set; } // Fill type: full, half, etc.
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
        public  string LadingState { get; set; } // State of the lading: "In Progress", "Completed"

       
        [Column(TypeName = "nvarchar(100)")]
        // New ShippingType column as Enum
        public  string ShippingType { get; set; } // Type of shipping: Air, Sea, Road, etc.

        
        // Weight Status as String
        [Column(TypeName = "nvarchar(100)")]
        public  string WeightStatus { get; set; } // Status of the weighing process: "Pending", "FinishedFirstWeight", "FinishedSecondWeight", "Finished"

        [Column(TypeName = "datetime2(0)")]
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;

        public int UserID { get; set; } // Foreign Key to user who created the record

        public int TransCompanyID { get; set; }  // Foreign key to TransCompany
        public TransCompany? TransCompany { get; set; }  // Navigation property
        // Navigation properties
        public virtual User? FirstWeigher { get; set; }
        public virtual User? SecondWeigher { get; set; }
        public virtual User? User { get; set; } // The user who created the OutLading
        public virtual Truck? Truck { get; set; }
        public virtual Driver? Driver { get; set; }
        public virtual Order? Order { get; set; } // Reference to the related Order

        // Method to calculate NetWeight and Shrink based on weights and quantity
        //public void CalculateNetWeightAndShrink()
        //{
        //    // Check if both weights are available and status is correct
        //    if (WeightStatus == "FinishedSecondWeight" && FirstWeight > 0 && SecondWeight > 0)
        //    {
        //        // Calculate NetWeight using absolute value to avoid negative net weight
        //        NetWeight = Math.Abs(SecondWeight - FirstWeight); // Calculate absolute NetWeight
        //        Shrink = Quantity - NetWeight; // Calculate Shrinkage based on quantity and net weight
        //        WeightStatus = "Finished"; // Update status to "Finished" once the calculation is complete
        //    }
        //}
    }

}
