using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Faculty : IEntity
    {
        public Guid Id { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }

        public Faculty()
        {
            Id = Guid.NewGuid();
        }
        public override string ToString()
        {
            return ShortName;
        }
    }
}
