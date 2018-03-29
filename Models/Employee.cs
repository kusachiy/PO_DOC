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
    public enum AcademicDegree
    {
        [Description("(нет)")]
        None,
        [Description("Доктор наук")]
        Doctor,
        [Description("Кандидат наук")]
        Candidate
    }
    public enum AcademicRank
    {
        [Description("(нет)")]
        None,
        [Description("Доцент")]
        Docent,
        [Description("Профессор")]
        Professor
    }
    public enum WorkingPosition
    {
        [Description("Профессор")]
        Professor,
        [Description("Доцент")]
        Docent,
        [Description("Старший преподаватель")]
        SeniorLecturer,
        [Description("Преподаватель")]
        Lecturer,
        [Description("Ассистент")]
        Assistant
    }

    public class Employee : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public AcademicDegree AcademicDegree { get; set; }
        [NotMapped]
        public string StringDegree => AcademicDegree.ToDescriptionString();
        public AcademicRank AcademicRank { get; set; }
        [NotMapped]
        public string StringRank => AcademicRank.ToDescriptionString();
        public WorkingPosition WorkingPosition { get; set; }//Job
        [NotMapped]
        public string StringPosition => WorkingPosition.ToDescriptionString();
        public float Rate { get; set; }
        public int ContractNumber { get; set; }
        public DateTime BirthDay { get; set; }
        public List<SpecialPosition> SpecialPositions { get; set; }

        public Employee()
        {
            Id = Guid.NewGuid();
            SpecialPositions = new List<SpecialPosition>();
        }
    }
}
