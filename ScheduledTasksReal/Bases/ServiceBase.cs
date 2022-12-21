using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.API.DbContexts;

namespace ScheduledTasks.Bases
{
    public class ServiceBase
    {
        protected BusinessContext _context;

        public ServiceBase(BusinessContext businessContext)
        {
            _context = businessContext;
        }
    }
}
