using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Core.DTOs
{
    public class UserInfoDTO
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public List<string> RoleGroups { get; set; }
        public List<string> Roles { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Guid CompanyId { get; set; }
    }
}
