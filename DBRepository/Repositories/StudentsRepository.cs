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
    public class StudentsRepository<TContext>: EntityRepository<Student>,IStudentsRepository where TContext:Context
    {
        public StudentsRepository(IAmbientDbContextLocator locator):base(locator)
        {

        }
    }
}
