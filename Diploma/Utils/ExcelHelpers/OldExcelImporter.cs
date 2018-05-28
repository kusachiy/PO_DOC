using System;
using System.Collections.Generic;
using System.Linq;
using Excel = Microsoft.Office.Interop.Excel;

using System.Text;
using System.Threading.Tasks;
using Models;
using Diploma.Services;
using CommonServiceLocator;
using System.Windows;

namespace Diploma.Utils.ExcelHelpers
{
    class OldExcelImporter:IImorter
    {
        Excel.Application xlApp;
        Excel.Workbook xlWorkBook;
        Excel.Worksheet xlWorkSheet;
        object misValue = System.Reflection.Missing.Value;
        public List<string> Log { get; set; }
        public List<Discipline> _newDisciplines { get; set; }
        IGeneralService _service;
        List<Group> _newGroups;
        List<DisciplineYear> _disciplines;
        List<DisciplineWorkload> _workloads;
        List<DisciplineYear> _disciplineYears;
        Department _defaultDepartment;
        List<Discipline> _allDisciplines;
        List<Semester> _semesters;
        List<StudyYear> _studyYears;
        List<Group> _groups;
        List<Speciality> _specialities;
        StudyYear _studyYear;

        public OldExcelImporter()
        {
            _service = ServiceLocator.Current.GetInstance<IGeneralService>();
            _disciplines = new List<DisciplineYear>();
            _newDisciplines = new List<Discipline>();
            _newGroups = new List<Group>();
            _workloads = new List<DisciplineWorkload>();
            _disciplineYears = new List<DisciplineYear>();
            _defaultDepartment = _service.GetAllDepartments().FirstOrDefault();
            _allDisciplines = _service.GetAllDisciplines();
            _semesters = _service.GetAllSemesters();
            _studyYears = _service.GetAllStudyYears();
            _groups = _service.GetAllGroups();
            _specialities = _service.GetAllSpecialities();
            Log = new List<string>();
        }
        public bool ImportDataFromExcel(string path, int year)
        {
            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Open(path, 0, true, 5, "", "", true, Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);                       
            if (_studyYear == null)
            {
                if (_studyYears.FirstOrDefault(s => s.Year == year) == null)
                    _studyYear = new StudyYear { Year = year };
                else
                    _studyYear = _studyYears.FirstOrDefault(s => s.Year == year);
            }           
            xlWorkSheet = xlWorkBook.Worksheets[1];
            int counter = Properties.OldImport.Default.StartRow;
            while (xlWorkSheet.GetCellText(counter, Properties.OldImport.Default.Semester) != "")
            {                
                string specialityName = "ПИН.РИС";
                string semester = xlWorkSheet.GetCellText(counter, Properties.OldImport.Default.Semester);
                string disciplineName = xlWorkSheet.GetCellText(counter, Properties.OldImport.Default.Discipline);
                int lectures = xlWorkSheet.GetCellText(counter, Properties.OldImport.Default.Lectures) != "" ? Convert.ToInt32(xlWorkSheet.GetCellText(counter, Properties.OldImport.Default.Lectures)) : 0;
                int labs = xlWorkSheet.GetCellText(counter, Properties.OldImport.Default.Labs) != "" ? Convert.ToInt32(xlWorkSheet.GetCellText(counter, Properties.OldImport.Default.Labs)) : 0;
                int practices = xlWorkSheet.GetCellText(counter, Properties.OldImport.Default.Practicies) != "" ? Convert.ToInt32(xlWorkSheet.GetCellText(counter, Properties.OldImport.Default.Practicies)) : 0;
                bool kr = xlWorkSheet.GetCellText(counter, Properties.OldImport.Default.KR) != "";
                bool kp = xlWorkSheet.GetCellText(counter, Properties.OldImport.Default.KP) != "";
                bool ekz = xlWorkSheet.GetCellText(counter, Properties.OldImport.Default.EKZ) != "";
                bool zach = xlWorkSheet.GetCellText(counter, Properties.OldImport.Default.ZACH) != "";

                Discipline discipline = _allDisciplines.FirstOrDefault(d => d.Name == disciplineName);
                if (discipline == null)
                {
                    discipline = _newDisciplines.FirstOrDefault(d => d.Name == disciplineName);
                    if (discipline == null)
                    {
                        bool isSpecial = !(kr || kp || ekz || zach);
                        bool hasWeeks = lectures + labs + practices != 0;
                        PracticeKind? type = null;
                        if (!hasWeeks && isSpecial)//выбрасываем всё кроме практик и учебных дисциплин
                        {
                            counter++;
                            continue;
                        }
                        else if (isSpecial)
                            type = GetSpecial(disciplineName);//определяем тип практики
                        discipline = new Discipline { Department = _defaultDepartment, Name = disciplineName,
                            PracticeType = type, TypeOfDiscipline = type==null?DisciplineType.EASY:DisciplineType.PRACTICE};
                        _newDisciplines.Add(discipline);
                        Log.Add($"Неизвестная дисциплина:'[{discipline.Name}]'");
                    }
                }

                DisciplineYear disciplineYear = new DisciplineYear { Discipline = discipline };

                disciplineYear.CountOfLecture = lectures;
                disciplineYear.CountOfPractice = practices;
                disciplineYear.CountOfLabs = labs;
                disciplineYear.HasKR = kr;
                disciplineYear.HasKP = kp;
                disciplineYear.HasEx = ekz;
                disciplineYear.HasCR = zach;

                _disciplineYears.Add(disciplineYear);
                int course = 1;
                if (semester != "")
                    course = (int)Math.Ceiling(Convert.ToInt32(semester) / 2f);
                int entryYear = (Convert.ToInt32(year) - course + 1);

                DisciplineWorkload workload = new DisciplineWorkload
                {
                    DisciplineYear = disciplineYear,
                    Semester = _semesters.FirstOrDefault(s => s.Number == Convert.ToInt32(semester))
                };
                if (workload.Semester == null)
                {
                    Log.Add($"ФАТАЛЬНО: Не найден семестр {semester}. Строка {counter}");
                    return false;
                }

                workload.StudyYear = _studyYear;
                var gr = _groups.FirstOrDefault(g => g.EntryYear == entryYear && g.Speciality.Name == specialityName);
                if (gr == null)
                {
                    var speciality = _specialities.FirstOrDefault(s => s.Name == specialityName);
                    if (speciality == null)
                    {
                        Log.Add($"ФАТАЛЬНО: Не найдена специальность {specialityName}. Строка {counter}");
                        return false;
                    }
                    if ((gr = _newGroups.FirstOrDefault(g => g.Speciality == speciality && g.EntryYear == entryYear)) == null)
                        _newGroups.Add(gr = new Group { EntryYear = entryYear, Name = $"{specialityName}{entryYear}", Speciality = speciality });
                    Log.Add($"Неизвестная группа {specialityName} поступившая в {entryYear} год. Строка {counter}");
                }
                workload.Group = gr;
                _workloads.Add(workload);
                counter++;
            }


            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();
            ReleaseObject(xlWorkSheet);
            ReleaseObject(xlWorkBook);
            ReleaseObject(xlApp);
            return true;
        }

        public void SaveData()
        {
            _service.AddOrUpdateStudyYear(_studyYear);
            foreach (var disc in _newDisciplines)
                _service.AddOrUpdateDiscipline(disc);
            foreach (var disYear in _disciplineYears)//долго 8с.
                _service.AddOrUpdateDisciplineYear(disYear);
            foreach (var group in _newGroups)
                _service.AddOrUpdateGroup(group);
            foreach (var workload in _workloads) //6с.
                _service.AddOrUpdateDisciplineWorkload(workload);
            InsertBaseDisciplines();
            WorkloadAssigmnent();
        }
        private void InsertBaseDisciplines()
        {
            List<Discipline> baseDisciplines = _service.GetAllSpecialDisciplines();
            var disciplineWorkloads = new List<DisciplineWorkload>();
            disciplineWorkloads.Add(new DisciplineWorkload {
                DisciplineYear = new DisciplineYear { Discipline = baseDisciplines.FirstOrDefault(d => d.SpecialType == SpecialDisciplineKind.GEK)},
                StudyYear = _studyYear, Semester = _semesters.FirstOrDefault(s => s.Number == 8) });
            disciplineWorkloads.Add(new DisciplineWorkload
            {
                DisciplineYear = new DisciplineYear
                {
                    Discipline = baseDisciplines.LastOrDefault(d => d.SpecialType == SpecialDisciplineKind.GEK)
                },
                StudyYear = _studyYear,
                Semester = _semesters.FirstOrDefault(s => s.Number == 12)
            });
            disciplineWorkloads.Add(new DisciplineWorkload
            {
                DisciplineYear = new DisciplineYear
                {
                    Discipline = baseDisciplines.FirstOrDefault(d => d.SpecialType == SpecialDisciplineKind.GAK)
                },
                StudyYear = _studyYear,
                Semester = _semesters.FirstOrDefault(s => s.Number == 8)
            });
            disciplineWorkloads.Add(new DisciplineWorkload
            {
                DisciplineYear = new DisciplineYear
                {
                    Discipline = baseDisciplines.FirstOrDefault(d => d.SpecialType == SpecialDisciplineKind.GAK_PRED)
                },
                StudyYear = _studyYear,
                Semester = _semesters.FirstOrDefault(s => s.Number == 8)
            });
            disciplineWorkloads.Add(new DisciplineWorkload
            {
                DisciplineYear = new DisciplineYear
                {
                    Discipline = baseDisciplines.FirstOrDefault(d => d.SpecialType == SpecialDisciplineKind.BAK_RUK)
                },
                StudyYear = _studyYear,
                Semester = _semesters.FirstOrDefault(s => s.Number == 8)
            });
            disciplineWorkloads.Add(new DisciplineWorkload
            {
                DisciplineYear = new DisciplineYear
                {
                    Discipline = baseDisciplines.FirstOrDefault(d => d.SpecialType == SpecialDisciplineKind.MAG_RUK)
                },
                StudyYear = _studyYear,
                Semester = _semesters.FirstOrDefault(s => s.Number == 12)
            });
            disciplineWorkloads.Add(new DisciplineWorkload
            {
                DisciplineYear = new DisciplineYear
                {
                    Discipline = baseDisciplines.FirstOrDefault(d => d.SpecialType == SpecialDisciplineKind.MAG_RETZ)
                },
                StudyYear = _studyYear,
                Semester = _semesters.FirstOrDefault(s => s.Number == 12)
            });
            disciplineWorkloads.Add(new DisciplineWorkload
            {
                DisciplineYear = new DisciplineYear
                {
                    Discipline = baseDisciplines.FirstOrDefault(d => d.SpecialType == SpecialDisciplineKind.RUK_KAF)
                },
                StudyYear = _studyYear                
            });
            _service.SaveDisciplineWorkloads(disciplineWorkloads);
            _workloads.AddRange(disciplineWorkloads);
        }
        public void WorkloadAssigmnent()
        {
            Log = new List<string>();
            foreach (var dWorkload in _workloads)
            {
                Guid? employeeId = _service.GetLastWorkloadEmployeeId(dWorkload.DisciplineYear.Discipline, dWorkload.Group, _studyYear);
                Workload workload;
                if (employeeId != null)
                    workload = new Workload { EmployeeId = employeeId, LocalWorkloadId = dWorkload.Id };
                else
                {
                    workload = new Workload { LocalWorkloadId = dWorkload.Id };
                    Log.Add($"Не удалось установить нагрузку [{dWorkload.DisciplineYear.Discipline.Name}] по данным прошлых лет");
                }
                _service.AddWorkload(workload);
            }
        }
        private static void ReleaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Unable to release the Object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        private static PracticeKind? GetSpecial(string disciplineName)
        {
            disciplineName = disciplineName.ToLower();
            if (disciplineName.Contains("уч") && disciplineName.Contains("практ"))
                return PracticeKind.LearningPractice;
            if (disciplineName.Contains("преддип") && disciplineName.Contains("практ"))
                return PracticeKind.UndergraduatePractice;
            if ((disciplineName.Contains("нир") || (disciplineName.Contains("ниир")) && disciplineName.Contains("практ") || (disciplineName.Contains("науч")&& (disciplineName.Contains("иссл")))))
                return PracticeKind.NIIR;
            if (disciplineName.Contains("произв") && disciplineName.Contains("практ"))
                return PracticeKind.ManufacturePractice;
            return null;
        }
    }
}
