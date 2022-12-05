using Web.API.Bases;

namespace Web.API.Models
{
    public class Role : EntityBase
    {
        public string Description { get; set; }
        public virtual ICollection<RoleGroup> RoleGroups { get; set; }
    }
}
