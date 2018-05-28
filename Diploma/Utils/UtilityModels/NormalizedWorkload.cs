using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Utils.UtilityModels
{
    class NormalizedWorkload
    {
        //Аудиторные занятия
        public float LectureCost { get; set; }
        public float PracticeCost { get; set; }
        public float LabsCost { get; set; }

        //Консультации
        public float LearningConsultationCost { get; set; }
        public float ExamConsultationCost { get; set; }

        //Контроль
        public float ExamControlCost { get; set; }
        public float ZachControlCost { get; set; }
        //public float RetzKRZaochCost { get; set; }//Рецензирование контрольных работ студентов-заочников
        public float GEKControlCost { get; set; }
        public float RetzMagDissCost { get; set; }
        //public float RetzAspNKRCost { get; set; }
        public float GAKControlCost { get; set; }

        //Практика
        public float LearningPracticeCost { get; set; }
        public float ManufacturePracticeCost { get; set; }
        public float ZaochPracticeCost { get; set; }
        public float UndergraduatePracticeCost { get; set; }
        //public float AspPracticeCost { get; set; }

        //Руководство
        public float KRControlCost { get; set; }//Руководство и  защита курсовых работ
        public float KPControlCost { get; set; }//Руководство и защита курсовых проектов
        public float BachelorVKRRukCost { get; set; }//Руководство выпускных квалификационных работ БАК
        public float MagistracyVKRRukCost { get; set; }//Руководство выпускных квалификационных работ МАГ
        public float AspNIIRRukCost { get; set; } //Руководство НИИР аспирантов

        //Дополнительно
        public float RukKafCost { get; set; }

        public NormalizedWorkload()
        {

        }
        public NormalizedWorkload(DisciplineWorkload disciplineWorkload)
        {
            var year = disciplineWorkload.DisciplineYear;
            var students = disciplineWorkload.Group.CountOfStudents;
            var weeks = disciplineWorkload.Semester.CountOfWeeks;
            var settings = Properties.CalculationSettings.Default;
            var studyForm = disciplineWorkload.Group.StudyForm;

            LectureCost = year.CountOfLecture * weeks * settings.LectureCost;
            PracticeCost = year.CountOfPractice * weeks * settings.PracticeCost;
            LabsCost = year.CountOfLabs * weeks * settings.LabCost;

            LearningConsultationCost = year.CountOfLecture * weeks * settings.KonsCost;
            if (studyForm != StudyForm.FullTime)
                LearningConsultationCost *= 3;
            ExamConsultationCost = year.HasEx ? settings.ExamConsCost : 0;

            //ExamControlCost = studyForm.FullTime? students * settings.ExamControlCost : students * settings.RGR;
            //Откуда 0.4f???
            ExamControlCost = year.HasEx ? (studyForm == StudyForm.FullTime ? students * settings.ExamControlCost : students * 0.4f):0;
            ZachControlCost = year.HasCR ? students * settings.ZachCost:0;

            KRControlCost = year.HasKR? settings.KRCost * students:0;
            KPControlCost = year.HasKP ? settings.KPCost * students : 0;
            

            switch (year.Discipline.SpecialType)
            {
                case SpecialDisciplineKind.GEK:
                    GEKControlCost = year.Discipline.SpecialType == SpecialDisciplineKind.GEK ? settings.GEK * students : 0;
                    break;
                case SpecialDisciplineKind.MAG_RETZ:
                    RetzMagDissCost = settings.MagRetz * students;
                    break;
                case SpecialDisciplineKind.GAK:
                    GAKControlCost = settings.GAK * students;
                    break;
               /* case SpecialDisciplineKind.LearningPractice:
                    LearningPracticeCost = settings.UchPr * year.CountOfLearnigPracticeWeeks;
                    break;
                case SpecialDisciplineKind.ManufacturePractice:
                    LearningPracticeCost = settings.PrPr * year.CountOfManufacturePracticeWeeks;
                    break;
                case SpecialDisciplineKind.UndergraduatePractice:
                    LearningPracticeCost = settings.PreddipPr * students;
                    break;*/
                case SpecialDisciplineKind.BAK_RUK:
                    BachelorVKRRukCost = settings.DPruk * students;
                    break;
                case SpecialDisciplineKind.MAG_RUK:
                    MagistracyVKRRukCost = settings.MAGRuk * students;
                    break;
                case SpecialDisciplineKind.ASP_RUK:
                    AspNIIRRukCost = settings.AspRuk * students;
                    break;
                case SpecialDisciplineKind.RUK_KAF:
                    AspNIIRRukCost = settings.RukKaf;
                    break;
                default:
                    break;
            }
        }
    }
}
