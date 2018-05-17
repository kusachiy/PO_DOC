using Diploma.Properties;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Utils
{
    public static class WorkloadsCalculator
    {
        public static float GetWorkloadCost(DisciplineWorkload workload)
        {           
            DisciplineYear disciplineYear = workload.DisciplineYear;
            Group group = workload.Group;
            Semester semester = workload.Semester;

            float workloadCost = 0;
            workloadCost += CalculationSettings.Default.LectureCost * disciplineYear.CountOfLecture * semester.CountOfWeeks;
            workloadCost += CalculationSettings.Default.LabCost * disciplineYear.CountOfLabs * semester.CountOfWeeks;
            workloadCost += CalculationSettings.Default.PracticeCost * disciplineYear.CountOfPractice * semester.CountOfWeeks;

            //Расчет консультаций по дисциплине
            if (group.StudyForm == StudyForm.FullTime)//очно           
                workloadCost += CalculationSettings.Default.KonsCost * disciplineYear.CountOfLecture * semester.CountOfWeeks;           
            else           
                workloadCost += CalculationSettings.Default.KonsCost * 3 * disciplineYear.CountOfLecture* semester.CountOfWeeks;
            if (disciplineYear.HasKR)            
                workloadCost += CalculationSettings.Default.KRCost * group.CountOfStudents;
            if (disciplineYear.HasKP)            
                workloadCost += CalculationSettings.Default.KPCost * group.CountOfStudents;
            if (disciplineYear.HasEx)
            {
                if (group.StudyForm == StudyForm.FullTime)                
                    workloadCost += CalculationSettings.Default.ExamControlCost * group.CountOfStudents + 2;                
                else                
                    workloadCost += 0.4f * group.CountOfStudents + 2;                
            }
            if (disciplineYear.HasCR)            
                workloadCost += CalculationSettings.Default.ZachCost * group.CountOfStudents;           

            workloadCost += CalculationSettings.Default.UchPr * disciplineYear.CountOfLearnigPracticeWeeks;
            workloadCost += CalculationSettings.Default.PrPr * disciplineYear.CountOfManufacturePracticeWeeks;
            workloadCost += CalculationSettings.Default.NIIR * disciplineYear.CountOfNIIR * group.CountOfStudents;
            switch (disciplineYear.Discipline.SpecialType)
            {
                case SpecialDisciplineKind.ASP_RUK:
                    workloadCost += CalculationSettings.Default.AspRuk;
                    break;
                case SpecialDisciplineKind.GEK:
                    workloadCost += CalculationSettings.Default.GEK * group.CountOfStudents*6;
                    break;
                case SpecialDisciplineKind.GAK:
                    workloadCost += CalculationSettings.Default.GAK * group.CountOfStudents * 6;
                    break;
                case SpecialDisciplineKind.BAK_RUK:
                    workloadCost += CalculationSettings.Default.DPruk;
                    break;               
                default:
                    break;
            }     
          
            return workloadCost;
        }

        /*public static float GetEmployeeAllWorkloadsCost(int employeeID, int yearID)
        {
            List<Workload> workloads = DataManager.SharedDataManager().GetEmployeeWorkloads(employeeID, yearID);

            float result = 0;

            foreach (Workload workload in workloads)
            {
                result += GetWorkloadCost(workload.ID);
            }

            return result;
        }*/
    }
}
