namespace EXONLOG.Model.Outbound
{
    using EXONLOG.Model.Account;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Port
    {
        [Key]
        public int PortID { get; set; } // Primary Key

        [Required]
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; } // Port Name

        [MaxLength(250)]
        [Column(TypeName = "nvarchar(100)")]
        public string Location { get; set; } // Port Location

        [Column(TypeName = "nvarchar(250)")]
        public string Notes { get; set; } // Additional Notes

        public DateTime CreateDate { get; set; } = DateTime.UtcNow; // Date the Port was Created

        [Required]
        public int UserID { get; set; } // Foreign Key to the User who created the port
        public User User { get; set; } // Navigation Property for User
    }

}
