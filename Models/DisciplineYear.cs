using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    

    public class DisciplineYear : IEntity
    {
        public Guid Id { get; set; }
        public Guid DisciplineId { get; set; }
        [ForeignKey("DisciplineId")]
        public Discipline Discipline { get; set; }

        public int CountOfLecture { get; set; }
        public int CountOfPractice { get; set; }
        public int CountOfLabs { get; set; }
        public int TotalLessons => CountOfLabs + CountOfLecture + CountOfPractice;

        public int CountOfLearnigPracticeWeeks => Discipline?.PracticeType == PracticeKind.LearningPractice ? TotalLessons : 0;
        public int CountOfManufacturePracticeWeeks => Discipline?.PracticeType == PracticeKind.ManufacturePractice ? TotalLessons : 0;
        public int CountOfUndergraduatePracticeWeeks => Discipline?.PracticeType == PracticeKind.UndergraduatePractice ? TotalLessons : 0;
        public int CountOfNIIR => Discipline?.PracticeType == PracticeKind.NIIR ? TotalLessons : 0;



        public bool HasKZ { get; set; }
        public bool HasKR { get; set; }
        public bool HasKP { get; set; }
        public bool HasEx { get; set; }
        public bool HasCR { get; set; }
        public bool HasCons { get; set; }
        
        public int? SpecialParameter { get; set; }

        public DisciplineYear()
        {
            Id = Guid.NewGuid();
        }
        public override string ToString()
        {
            return $"{Discipline.Name}";
        }
    }
}
