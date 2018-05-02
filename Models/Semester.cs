using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public enum SemesterType
    {
        Autumm,Spring
    }
    public class Semester : IEntity
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public int CountOfWeeks { get; set; }
        [NotMapped]
        public SemesterType Type => Number % 2 == 0 ? SemesterType.Spring : SemesterType.Autumm;
        public Semester()
        {
            Id = Guid.NewGuid();
        }
        public override string ToString()
        {
            return Number.ToString();
        }
    }
}
