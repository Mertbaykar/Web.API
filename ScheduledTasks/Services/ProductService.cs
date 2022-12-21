using ScheduledTasksOld.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.API.DbContexts;
using Web.API.Models;

namespace ScheduledTasksOld.Services
{
    public class ProductService : ServiceBase
    {
        public ProductService(BusinessContext businessContext) : base(businessContext)
        {

        }

        public List<Product> ShowProducts()
        {
            return _context.Products.ToList();
        }
    }
}
