using EXONLOG.Model.Account;
using EXONLOG.Model.Trans;
using System.ComponentModel.DataAnnotations.Schema;

namespace EXONLOG.Model.Inbound
{
    public class InLading
    {
        public int ID { get; set; } // Primary Key

        public string LadingRef { get; set; } = string.Empty; // Unique reference for the lading

        public int BatchID { get; set; } // Foreign key to the batch

        public int TruckID { get; set; } // Foreign key to the truck

        public int DriverID { get; set; } // Foreign key to the driver

        public double FirstWeight { get; set; } // Weight at the first weighbridge

        public double SecondWeight { get; set; } // Weight at the second weighbridge

        public double NetWeight { get; set; } // Net weight (calculated as |SecondWeight - FirstWeight|)

        public DateTime FirstWeighDate { get; set; } // Date and time of the first weight

        public DateTime SecondWeighDate { get; set; } // Date and time of the second weight

        [Column(TypeName = "nvarchar(100)")]
        public string WeightStatus { get; set; } = "Pending"; // Status of the weigh operation

        public int FirstWeigherID { get; set; } // User ID of the first weigher

        public int SecondWeigherID { get; set; } // User ID of the second weigher

        [Column(TypeName = "nvarchar(250)")]
        public string? Notes { get; set; } // Additional notes for the lading

        public DateTime CreateDate { get; set; } = DateTime.Now; // Record creation date

        public int UserID { get; set; } // Foreign key to the user who created the record

        public int TransCompanyID { get; set; }  // Foreign key to TransCompany
        public TransCompany TransCompany { get; set; }  // Navigation property
        // Navigation Properties
        public Batch? Batch { get; set; } // Related Batch
        public Truck? Truck { get; set; } // Related Truck
        public Driver? Driver { get; set; } // Related Driver
        public User? User { get; set; } // User who created the lading
        public User? FirstWeigher { get; set; } // First weigher user
        public User? SecondWeigher { get; set; } // Second weigher user
    }

}
