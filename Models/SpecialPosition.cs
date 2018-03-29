using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class SpecialPosition : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid ExecutorId { get; set; }
        [ForeignKey("ExecutorId")]
        public Employee Executor { get; set; }

        public SpecialPosition()
        {
            Id = Guid.NewGuid();
        }
    }
}
