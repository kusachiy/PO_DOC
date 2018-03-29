using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Department:IEntity
    {
        public Guid Id { get; set; }
        public int CodeNumber { get; set; }
        public string Description { get; set; }
        
        public Department()
        {
            Id = Guid.NewGuid();
        }
        public override string ToString()
        {
            return Description;
        }
    }
}
