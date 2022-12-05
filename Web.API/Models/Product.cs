using Web.API.Bases;

namespace Web.API.Models
{
    public class Product : EntityBase
    {
        public Product()
        {
            Categories = new List<Category>();
            Companies= new List<Company>();
        }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Company> Companies { get; set; }
    }
}
