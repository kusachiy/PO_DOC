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
    public enum DisciplineType
    {
        EASY,PRACTICE,SPECIAL
    }

    public enum PracticeKind
    {
        [Description("Учебная практика")]
        LearningPractice,
        [Description("Производственная практика")]
        ManufacturePractice,
        [Description("Преддипломная практика")]
        UndergraduatePractice,
        [Description("НИИР")]
        NIIR
    }

    public enum SpecialDisciplineKind
    {         
        [Description("ГЭК")]
        GEK,
        [Description("ГАК")]
        GAK,
        [Description("Председатель ГАК")]
        GAK_PRED,
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
        [Description("Нормоконтроль магистров")]
        NCTRL_MAG,
        [Description("Допуск к диссертации магистров")]
        DOPUSK_DISS,
    }

    public class Discipline : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public Department Department { get; set; } // Могут быть конфликты из-за неправильной схемы данных.
        public DisciplineType TypeOfDiscipline { get; set; }
        public SpecialDisciplineKind? SpecialType { get; set; }
        public PracticeKind? PracticeType { get; set; }
        public string StringSpecialType => SpecialType?.ToDescriptionString() ?? "";
        public string StringPracticeType => PracticeType?.ToDescriptionString() ?? "";

        public int CountOfWorkloads { get; set; }
        public bool IsActiveDiscipline { get; set; }

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
            CountOfWorkloads = 1;
            IsActiveDiscipline = true;
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
