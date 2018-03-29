using DBRepository.Workers;
using DBRepository.Workers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Services
{
    public abstract class ServiceBase : Disposable
    {
        protected ServiceBase(IDbWorker dbWorker)
        {
            Db = dbWorker;
        }

        public IDbWorker Db { get; private set; }

        protected virtual void DoWork(Action action)
        {
            try
            {
                using (var scope = Db.BeginWork())
                {
                    action();
                    scope.SaveChanges();
                }
            }
            catch (Exception)
            {
                
            }
        }

        protected virtual void DoReadOnlyWork(Action action)
        {
            try
            {
                using (Db.BeginReadOnlyWork())
                {
                    action();
                }
            }
            catch (Exception)
            {
                
            }
        }
    }
}
