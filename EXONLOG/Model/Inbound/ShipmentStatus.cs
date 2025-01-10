using System.ComponentModel.DataAnnotations.Schema;

namespace EXONLOG.Model.Inbound
{
    public class ShipmentStatus
    {
        public int ShipmentStatusId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string StatusName { get; set; }

        public ICollection<Shipment> Shipments { get; set; }
    }

}
