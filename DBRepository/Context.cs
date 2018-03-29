using Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBRepository
{
    public class Context : DbContext
    {
        public Context(string connectionString)
            : base(connectionString)
        { }
        public Context()
            : base("AccessConnection") { }
        public DbSet<Student> Students { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Department> Departments {get;set;}
        public DbSet<Discipline> Discipline { get; set; }
        public DbSet<DisciplineYear> DisciplineYears { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Semester> Semesters{ get; set; }
        public DbSet<Speciality> Specialities { get; set; }
        public DbSet<StudyYear> StudyYears { get; set; }
        public DbSet<Workload> Workloads { get; set; }
        public DbSet<DisciplineWorkload> LocaLWorkloads { get; set; }
        public DbSet<SpecialPosition> SpecialPositions { get; set; }
    }
}
