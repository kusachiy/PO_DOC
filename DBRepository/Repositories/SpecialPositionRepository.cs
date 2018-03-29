using DBRepository.Repositories.Interfaces;
using Mehdime.Entity;
using Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBRepository.Repositories
{
    public class SpecialPositionRepository<TContext>: EntityRepository<SpecialPosition>, ISpecialPositionRepository where TContext:Context
    {
        public SpecialPositionRepository(IAmbientDbContextLocator locator):base(locator)
        {

        }
    }
}
