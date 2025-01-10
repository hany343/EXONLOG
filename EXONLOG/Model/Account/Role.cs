using System.ComponentModel.DataAnnotations.Schema;

namespace EXONLOG.Model.Account
{
    public class Role
    {
        public int RoleId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string RoleName { get; set; }

        public ICollection<User> Users { get; set; }
    }

}
