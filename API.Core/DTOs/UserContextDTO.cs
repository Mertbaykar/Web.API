using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Core.DTOs
{
    public class UserContextDTO
    {
        public Guid Id { get; protected set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public List<UserRoleGroupDTO> RoleGroups { get; set; }
        public List<UserRoleDTO> Roles { get; set; }

    }
}
