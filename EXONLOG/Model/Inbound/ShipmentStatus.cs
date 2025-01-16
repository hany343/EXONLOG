using EXONLOG.Model.Account;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EXONLOG.Model.Inbound
{
    public class ShipmentStatus
    {
        public int ShipmentStatusId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string StatusName { get; set; } = string.Empty;

        public DateTime CreateDate { get; set; } = DateTime.UtcNow; // Record creation date

        public ICollection<Shipment>? Shipments { get; set; }

        [Required]
        public int UserID { get; set; } // Foreign Key to the User who created the material
        public User? User { get; set; } // Navigation Property for User
    }

}
