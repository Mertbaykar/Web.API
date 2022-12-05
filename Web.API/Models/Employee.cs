using System.ComponentModel.DataAnnotations.Schema;
using Web.API.Bases;

namespace Web.API.Models
{
    public class Employee : EntityBase
    {
        public string LastName { get; set; }
        [NotMapped]
        public string FullName => Name + " " + LastName;
        public virtual ICollection<RoleGroup> RoleGroups { get; set; }
        public virtual Company Company { get; set; }
        [ForeignKey("Company")]
        public Guid CompanyId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
    }
}
