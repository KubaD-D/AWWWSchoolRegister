using DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.UnitTests
{
    public abstract class BaseUnitTests
    {
        protected readonly ApplicationDbContext DbContext;

        public BaseUnitTests(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }
    }
}
