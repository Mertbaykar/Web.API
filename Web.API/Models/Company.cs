using System.ComponentModel.DataAnnotations.Schema;
using Web.API.Bases;

namespace Web.API.Models
{
    public class Company : EntityBase
    {
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
