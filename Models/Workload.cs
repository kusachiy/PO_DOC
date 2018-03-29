using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Workload : IEntity
    {
        public Guid Id { get; set; }
        public Guid LocalWorkloadId { get; set; }
        [ForeignKey("LocalWorkloadId")]
        public DisciplineWorkload LocalWorkload { get; set; }
        public Guid? EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }
        //public int CountOfStudents => Group.Students.Count;
        public int CountOfStudents { get; set; }
        //public int CountOfWeeks => Semester.CountOfWeeks;
        public int CountOfWeeks { get; set; }

        public Workload()
        {
            Id = Guid.NewGuid();
        }
    }
}
