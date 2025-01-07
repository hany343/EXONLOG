using EXONLOG.Data.Trans;
using System.ComponentModel.DataAnnotations;

namespace EXONLOG.Data.Entities.Outbound
{
    public class OutboundLading
    {
        [Key]
        public int LadingID { get; set; }

        [Required]
        public int TruckID { get; set; } // Foreign Key to Truck

        [Required]
        public int DriverID { get; set; } // Foreign Key to Driver

        [Required]
        public int OrderID { get; set; } // Foreign Key to Order

        [Required]
        public int TransCompanyID { get; set; } // Foreign Key to TransCompany

        public string Type { get; set; } // "Inbound" or "Outbound"

        // First Weight Information
        public double FirstWeight { get; set; } // Weight after first weighbridge
        public int? FirstWeigherID { get; set; } // Foreign Key to User who performed the first weigh
        public DateTime? FirstWeightDate { get; set; } // Date and time of first weighing

        // Second Weight Information
        public double SecondWeight { get; set; } // Weight after second weighbridge
        public int? SecondWeigherID { get; set; } // Foreign Key to User who performed the second weigh
        public DateTime? SecondWeightDate { get; set; } // Date and time of second weighing

        // Net Weight Information
        public double NetWeight { get; set; } // Net weight (auto-calculated)
        public double Shrink { get; set; } // Shrinkage between quantity and net weight (auto-calculated)

        // Additional Fields
        public double Quantity { get; set; } // Quantity of materials
        public string FillType { get; set; } // Fill type: full, half, etc.
        public string StackBar { get; set; } // Stack bar identifier
        public int BagsCount { get; set; } // Number of bags in the shipment
        public string ShippingAddress { get; set; } // Shipping address for delivery
        public string ShippingCity { get; set; } // City of the shipping address
        public DateTime? LeaveDate { get; set; } // Date when goods leave
        public int ApprovedUserID { get; set; } // User who approves the lading
        public string RepresentativeName { get; set; } // Name of the representative for the shipment
        public string WeighNotes { get; set; } // Additional notes related to the weigh process
        public string LadingState { get; set; } // State of the lading: "In Progress", "Completed"

        // New ShippingType column as Enum
        public string ShippingType { get; set; } // Type of shipping: Air, Sea, Road, etc.

        // Weight Status as String
        public string WeightStatus { get; set; } // Status of the weighing process: "Pending", "FinishedFirstWeight", "FinishedSecondWeight", "Finished"

        public DateTime CreateDate { get; set; } = DateTime.UtcNow;

        public int UserID { get; set; } // Foreign Key to user who created the record

        // Navigation properties
        public virtual User FirstWeigher { get; set; }
        public virtual User SecondWeigher { get; set; }
        public virtual User User { get; set; } // The user who created the OutboundLading
        public virtual Truck Truck { get; set; }
        public virtual Driver Driver { get; set; }
        public virtual Order Order { get; set; } // Reference to the related Order

        // Method to calculate NetWeight and Shrink based on weights and quantity
        public void CalculateNetWeightAndShrink()
        {
            // Check if both weights are available and status is correct
            if (WeightStatus == "FinishedSecondWeight" && FirstWeight > 0 && SecondWeight > 0)
            {
                // Calculate NetWeight using absolute value to avoid negative net weight
                NetWeight = Math.Abs(SecondWeight - FirstWeight); // Calculate absolute NetWeight
                Shrink = Quantity - NetWeight; // Calculate Shrinkage based on quantity and net weight
                WeightStatus = "Finished"; // Update status to "Finished" once the calculation is complete
            }
        }
    }

}
