using Models.Helpers;
using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{   
    public enum DisciplineType
    {
        [Description("Простая")]
        Simple,
        [Description("Специальная по числу студентов")]
        SpecialStudentCount,
        [Description("Специальная по количеству недель")]
        SpecialWeeks
    }
    
    public class Discipline : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        /*[NotMapped]
        public DisciplineType TypeOfDiscipline { get; set; }
        public string StringType => TypeOfDiscipline.ToDescriptionString();*/
        public Guid DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public Department Department { get; set; } // Могут быть конфликты из-за неправильной схемы данных.
        public bool IsSpecial { get; set; }
        public string StringSpecial => IsSpecial?"Специальная":"Простая";      

        public Discipline()
        {
            Id = Guid.NewGuid();
        }
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
