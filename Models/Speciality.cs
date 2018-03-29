using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Speciality : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid FacultyId { get; set; }
        [ForeignKey("FacultyId")]
        public Faculty Faculty{ get; set; }

        public Speciality()
        {
            Id = Guid.NewGuid();
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
