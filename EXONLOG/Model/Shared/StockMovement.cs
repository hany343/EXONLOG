using EXONLOG.Model.Account;
using System.ComponentModel.DataAnnotations.Schema;

namespace EXONLOG.Model.Shared
{
    public class StockMovement
    {
        public int StockMovementId { get; set; }

        public int StockId { get; set; }
        
        [Column(TypeName = "nvarchar(100)")]
        public string MovementType { get; set; } // Inbound or Outbound

        public double QuantityChanged { get; set; }

        public DateTime MovementDate { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }
        public Stock Stock { get; set; }

    }

}
