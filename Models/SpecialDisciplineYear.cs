using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class SpecialDisciplineYear:IEntity
    {
        public Guid Id { get; set; }
        public Guid SpecialDisciplineId { get; set; }
        [ForeignKey("SpecialDisciplineId")]
        public SpecialDiscipline SpecialDiscipline { get; set; }
        public int? Parameter { get; set; }

        public SpecialDisciplineYear()
        {
            Id = Guid.NewGuid();
        }
    }
}
