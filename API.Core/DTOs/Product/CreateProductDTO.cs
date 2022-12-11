using API.Core.Bases;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Core.DTOs.Product
{
    public class CreateProductDTO
    {
        [Required(ErrorMessage ="Ad boş bırakılamaz")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Açıklama boş bırakılamaz")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Fiyat boş bırakılamaz ve sayı girilmelidir")]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal Price { get; set; }

        public List<Guid> Categories { get; set; } = new List<Guid>();
        public List<Guid> Companies { get; set; } = new List<Guid>();
    }
}
