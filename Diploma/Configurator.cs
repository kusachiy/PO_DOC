using Autofac;
using Autofac.Extras.CommonServiceLocator;
using CommonServiceLocator;
using DBRepository;
using DBRepository.Factories;
using DBRepository.Repositories;
using DBRepository.Repositories.Interfaces;
using DBRepository.Workers;
using DBRepository.Workers.Interfaces;
using Diploma.Services;
using Mehdime.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Diploma
{
    class Configurator
    {
        public Configurator()
        {
            var builder = new ContainerBuilder();
            ConfigureContainer(builder);
            var autofacLocator = new AutofacServiceLocator(builder.Build());
            ServiceLocator.SetLocatorProvider(() => autofacLocator);

        }      

        protected void ConfigureContainer(ContainerBuilder builder)
        {
            Assembly assembly = typeof(App).Assembly;
            builder.RegisterAssemblyTypes(assembly)
                .Where(item => item.Name.EndsWith("Manager") && item.IsAbstract == false)
                .AsSelf()
                .SingleInstance();

            builder.RegisterType<DbContextScopeFactory>().As<IDbContextScopeFactory>().SingleInstance();
            builder.RegisterType<AmbientDbContextLocator>().As<IAmbientDbContextLocator>().SingleInstance();
            builder.RegisterType<DbWorker>().As<IDbWorker>().SingleInstance();
            //builder.RegisterType<SQLSERVERContextFactory>().As<IDbContextFactory>().SingleInstance();
            builder.RegisterType<AccessContextFactory>().As<IDbContextFactory>().SingleInstance();
            // Repository registration
            RegisterRepositories<Context>(builder);

            // Service registration
            builder.RegisterAssemblyTypes(typeof(GeneralService).Assembly)
                 .Where(t => t.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerDependency();
        }
        public void RegisterRepositories<TContext>(ContainerBuilder builder)
               where TContext : Context
        {
            builder.RegisterType<DepartmnentsRepository<TContext>>().As<IDepartmentRepository>().InstancePerDependency();
            builder.RegisterType<DisciplineRepository<TContext>>().As<IDisciplineRepository>().InstancePerDependency();
            builder.RegisterType<DisciplineYearRepository<TContext>>().As<IDisciplineYearRepository>().InstancePerDependency();
            builder.RegisterType<DisciplineWorkloadRepository<TContext>>().As<IDisciplineWorkloadRepository>().InstancePerDependency();
            builder.RegisterType<EmployeeRepository<TContext>>().As<IEmployeeRepository>().InstancePerDependency();
            builder.RegisterType<FacultyRepository<TContext>>().As<IFacultyRepository>().InstancePerDependency();
            builder.RegisterType<SpecialityRepository<TContext>>().As<ISpecialityRepository>().InstancePerDependency();
            builder.RegisterType<StudyYearRepository<TContext>>().As<IStudyYearRepository>().InstancePerDependency();
            builder.RegisterType<WorkloadRepository<TContext>>().As<IWorkloadRepository>().InstancePerDependency();
            builder.RegisterType<SemesterRepository<TContext>>().As<ISemesterRepository>().InstancePerDependency();     
            builder.RegisterType<StudentsRepository<TContext>>().As<IStudentsRepository>().InstancePerDependency();
            builder.RegisterType<GroupsRepository<TContext>>().As<IGroupsRepository>().InstancePerDependency();
            builder.RegisterType<SpecialPositionRepository<TContext>>().As<ISpecialPositionRepository>().InstancePerDependency();


        }
    }
}
