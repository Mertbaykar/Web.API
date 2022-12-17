using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace API.Core.RoleDefinitions
{
    public static class RoleDefinitions
    {
        //private const string adminRoles = "Admin Yetkileri";
        private const string productRoles = "Ürün Yetkileri";




        #region Product

        [Display(Prompt = productRoles, Name = "Ürün Yönetme Yetkisi", Description = "Ürünlerle ilgili her türlü işlemi yapabilme yetkisidir")]
        public const string ProductAdmin = "ProductAdmin";

        [Display(Prompt = productRoles, Name = "Ürün İzleme Yetkisi", Description = "Ürün izleme yetkisidir")]
        public const string ProductReadOnly = "ProductReadOnly";

        [Display(Prompt = productRoles, Name = "Ürün Düzenleme Yetkisi", Description = "Ürün düzenleme yetkisidir")]
        public const string ProductEdit = "ProductEdit";

        [Display(Prompt = productRoles, Name = "Ürün Yaratma Yetkisi", Description = "Ürün yaratma yetkisidir")]
        public const string ProductCreate = "ProductCreate";
        #endregion
    }
}
