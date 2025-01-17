using EXONLOG.Model.Account;
using System.ComponentModel.DataAnnotations;

namespace EXONLOG.Model.Trans
{
    public class TransCompany
    {
        [Key]
        public int TransCompanyID { get; set; } // Primary Key

        [Required]
        [MaxLength(100)]
        public string CompanyName { get; set; } // MaterialName of the transport company

        [MaxLength(200)]
        public string Address { get; set; } // Address of the transport company

        [MaxLength(15)]
        public string Phone { get; set; } // Contact phone number

        [MaxLength(100)]
        public string Email { get; set; } // Contact email

        public DateTime CreateDate { get; set; } = DateTime.UtcNow; // Date the company was added

        public int UserID { get; set; } // Foreign Key to User who created the record
        public User? User { get; set; } // Navigation Property for User

    }

}
