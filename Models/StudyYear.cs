using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class StudyYear : IEntity
    {
        public Guid Id { get; set; }
        public int Year { get; set; }
        public StudyYear()
        {
            Id = Guid.NewGuid();
        }
        public override string ToString()
        {
            return string.Format($"{Year} - {Year+ 1}");
        }
    }
}
