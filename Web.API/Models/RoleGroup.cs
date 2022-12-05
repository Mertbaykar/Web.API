using Web.API.Bases;

namespace Web.API.Models
{
    public class RoleGroup : EntityBase
    {
        public string Description { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }

    }
}
