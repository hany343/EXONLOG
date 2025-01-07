namespace EXONLOG.Data.Entities.Outbound
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Port
    {
        [Key]
        public int PortID { get; set; } // Primary Key

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } // Port Name

        [MaxLength(250)]
        public string Location { get; set; } // Port Location

        public string Notes { get; set; } // Additional Notes

        public DateTime CreateDate { get; set; } = DateTime.UtcNow; // Date the Port was Created

        [Required]
        public int UserID { get; set; } // Foreign Key to the User who created the port
        public User User { get; set; } // Navigation Property for User
    }

}
