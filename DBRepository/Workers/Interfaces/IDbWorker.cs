using Mehdime.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBRepository.Workers.Interfaces
{
    public interface IDbWorker : IDisposable
    {
        /// <summary>
        /// Начать работу с базой данных, т.е. создать execution scope
        /// в режиме read/write
        /// 
        /// Типовое применение:
        /// 
        /// using(var scope = BeginWork())
        /// {
        ///     // Some work in repositories
        ///     scope.SaveChanges();
        /// }
        /// </summary>
        /// <returns></returns>
        IDbContextScope BeginWork();

        /// <summary>
        /// Начать работу с базой данных, т.е. создать execution scope
        /// в режиме read only
        /// 
        /// Типовое применение:
        /// 
        /// using(BeginReadOnlyWork())
        /// {
        ///     // Some work in repositories without write operations
        ///     // e.g. var users = userRepository.GetAll();
        /// }
        /// </summary>
        /// <returns></returns>
        IDbContextReadOnlyScope BeginReadOnlyWork();
    }
}
