using Models.Helpers;
using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{       
    public enum SpecialDisciplineKind
    {
        [Description("Учебная практика")]
        LearningPractice,
        [Description("Производственная практика")]
        ManufacturePractice,
        [Description("Преддипломная практика")]
        UndergraduatePractice,
        [Description("НИИР")]
        NIIR,
        [Description("ГЭК")]
        GEK,
        [Description("ГАК")]
        GAK,
        [Description("Руководство магистрами")]
        MAG_RUK,
        [Description("Руководство бакалаврами")]
        BAK_RUK,
        [Description("Руководство аспирантами")]
        ASP_RUK,
        [Description("Рецензирование магистранстких работ")]
        MAG_RETZ,
        [Description("Рецензирование аспирантских работ")]
        ASP_RETZ,
        [Description("Руководство кафедрой")]
        RUK_KAF,
    }

    public class Discipline : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        /*[NotMapped]
        public DisciplineType TypeOfDiscipline { get; set; }
        public string StringType => TypeOfDiscipline.ToDescriptionString();*/
        public Guid DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public Department Department { get; set; } // Могут быть конфликты из-за неправильной схемы данных.
        public bool IsSpecial { get; set; }
        public string StringSpecial => IsSpecial?"Специальная":"Простая"; 
        public SpecialDisciplineKind? SpecialType { get; set; }
        public string StringSpecialType => SpecialType?.ToDescriptionString() ?? "";
        [NotMapped]
        public int SpecialTypeSelector
        {
            get { return SpecialType == null ? -1 : (int)SpecialType; }
            set
            {
                if (value == -1)
                    SpecialType = null;
                else
                    SpecialType = (SpecialDisciplineKind)value;
            }
        }

        public Discipline()
        {
            Id = Guid.NewGuid();
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
