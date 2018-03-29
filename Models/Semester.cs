using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Semester : IEntity
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public int CountOfWeeks { get; set; }

        public Semester()
        {
            Id = Guid.NewGuid();
        }
    }
}
