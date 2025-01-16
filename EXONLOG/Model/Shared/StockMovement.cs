using EXONLOG.Model.Account;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EXONLOG.Model.Shared
{
    public class StockMovement
    {
        [Key]
        public int StockMovementId { get; set; }

        [Required]
        public int StockId { get; set; }
        public Stock? Stock { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string MovementType { get; set; } // Inbound or Outbound

        public double QuantityChanged { get; set; }

        public DateTime MovementDate { get; set; }


        [Required]
        public int UserID { get; set; } // User who created or updated the Stock
        public User? User { get; set; } // Navigation Property for User

        

    }

}
