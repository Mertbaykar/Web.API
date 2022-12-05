using Web.API.Bases;

namespace Web.API.Models
{
    public class Category: EntityBase
    {
        public string Description { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
