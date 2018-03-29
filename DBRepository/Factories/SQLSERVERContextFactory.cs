using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBRepository.Factories
{
    public class SQLSERVERContextFactory : SwitchableDbContextFactory
    {
        public SQLSERVERContextFactory():base("DefaultConnection")
        {

        }
    }
}
