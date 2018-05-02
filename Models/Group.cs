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
    public enum Qualification
    {
        [Description("Бакалавриат")]
        Bachelor,
        [Description("Специалитет")]
        Specialist,
        [Description("Магистратура")]
        Magistracy
    }
    public enum StudyForm
    {
        [Description("Очная")]
        FullTime,
        [Description("Очно-заочная")]
        CorrespondenceTime,
        [Description("Заочная")]
        PartTime
    }

    public class Group : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Student> Students { get; set; }
        public Qualification Qualification { get; set; }
        public string StringQualification => Qualification.ToDescriptionString();
        public StudyForm StudyForm { get; set; }
        public string StringStudyForm => StudyForm.ToDescriptionString();
        public Guid SpecialityId { get; set; }
        [ForeignKey("SpecialityId")]
        public Speciality Speciality { get; set; }

        ////////////////Надо убирать эти поля
        public int CountOfStudents { get; set; }////--------------- Точно
        public int EntryYear { get; set; }////--------------------- Наверно
        ////-------------------------------------------------------/////
        ////-------------------------------------------------------/////

        public Group()
        {
            Id = Guid.NewGuid();
            Students = new List<Student>();
        }
        public override string ToString()
        {
            return Name;
        }        
    }
}
