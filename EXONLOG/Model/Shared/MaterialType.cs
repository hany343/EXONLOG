using EXONLOG.Model.Account;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EXONLOG.Model.Shared
{
    public class MaterialType
    {

        [Key]
        public int MaterialTypeID { get; set; } // Primary Key

        [Required]
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string MaterialTypeName { get; set; } // Material Name

        [MaxLength(500)]
        [Column(TypeName = "nvarchar(100)")]
        public string? Description { get; set; } // Description of the Material

        [Column(TypeName = "nvarchar(200)")]
        public string? Notes { get; set; } // Additional Notes

        public DateTime CreateDate { get; set; } = DateTime.UtcNow; // Date the Material was Created

        [Required]
        public int UserID { get; set; } // Foreign Key to the User who created the material
        public User? User { get; set; } // Navigation Property for User


        // One-to-many relationship with Stock
        public ICollection<Material> Materials { get; set; } = new List<Material>(); // A material can have multiple stock entries

    }
}
