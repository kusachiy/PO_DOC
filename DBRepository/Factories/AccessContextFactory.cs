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
    public class AccessContextFactory : SwitchableDbContextFactory
    {
        public AccessContextFactory():base("AccessConnection")
        {

        }
    }    
}
