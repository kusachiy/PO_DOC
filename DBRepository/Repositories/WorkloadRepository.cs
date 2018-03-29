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
    public class WorkloadRepository<TContext>: EntityRepository<Workload>, IWorkloadRepository where TContext:Context
    {
        public WorkloadRepository(IAmbientDbContextLocator locator):base(locator)
        {

        }
    }
}
