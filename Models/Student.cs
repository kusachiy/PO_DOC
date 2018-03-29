using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{   
    public class Student : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? GroupId { get; set; }
        [ForeignKey("GroupId")]
        public Group Group { get; set; }
        public Student()
        {
            Id = Guid.NewGuid();
        }
    }
}
