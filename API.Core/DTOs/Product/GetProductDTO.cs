using API.Core.Bases;
using API.Core.DTOs.Category;
using API.Core.DTOs.Company;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Core.DTOs.Product
{
    public class GetProductDTO
    {
        [Display(Name = "Ürün")]
        public Guid Id { get; set; }
        [Display(Name = "Ürün Adı")]
        public string Name { get; set; }
        [Display(Name = "Açıklama")]
        public string Description { get; set; }
        [Display(Name = "Fiyat")]
        public decimal Price { get; set; }
        [Display(Name = "Kategoriler")]
        public List<GetCategoryDTO> Categories { get; set; } = new List<GetCategoryDTO>();
        [Display(Name = "Firma")]
        public GetCompanyDTO Company { get; set; }
    }
}
