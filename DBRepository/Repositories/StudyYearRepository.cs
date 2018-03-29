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
    public class StudyYearRepository<TContext>: EntityRepository<StudyYear>, IStudyYearRepository where TContext:Context
    {
        public StudyYearRepository(IAmbientDbContextLocator locator):base(locator)
        {

        }
    }
}
