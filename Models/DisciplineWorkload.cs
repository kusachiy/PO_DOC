using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class DisciplineWorkload : IEntity
    {
        public Guid Id { get; set; }
        public Guid DisciplineYearId { get; set; }
        [ForeignKey("DisciplineYearId")]
        public DisciplineYear DisciplineYear { get; set; }
        public Guid GroupId { get; set; }
        [ForeignKey("GroupId")]
        public Group Group { get; set; }            
        public Guid SemesterId { get; set; }
        [ForeignKey("SemesterId")]
        public Semester Semester { get; set; }
        public Guid StudyYearId { get; set; }
        [ForeignKey("StudyYearId")]
        public StudyYear StudyYear { get; set; }

        public DisciplineWorkload()
        {
            Id = Guid.NewGuid();
        }
        //_____________________________________
    }
}
