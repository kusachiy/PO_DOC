using CommonServiceLocator;
using Diploma.Properties;
using Diploma.Services;
using Microsoft.Office.Interop.Excel;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Diploma.Utils.ExcelHelpers
{
    class ExcelExporter
    {
        static IGeneralService _service;
        static ExcelExporter()
        {
            _service = ServiceLocator.Current.GetInstance<IGeneralService>();
        }
        public static void ExportIndividualPlan(Employee employee, StudyYear year)
        {
            string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Templates\IndPlanTemplate.xltx");
            Microsoft.Office.Interop.Excel.Application ObjExcel = new Microsoft.Office.Interop.Excel.Application();
            Workbook ObjWorkBook;
            Worksheet ObjWorkSheet;//Книга.
            ObjWorkBook = ObjExcel.Workbooks.Add(path);//System.Reflection.Missing.Value);          
            ObjWorkSheet = (Worksheet)ObjWorkBook.Sheets[1];
            ObjWorkBook.Title = string.Format("Индивидуальный план {0} ({1} / {2})", employee.Name, year.Year, Convert.ToInt32(year.Year) + 1);
            ObjWorkSheet.Cells[Properties.IndPlanExport.Default.YearsDescriptionRowCell.X,
                Properties.IndPlanExport.Default.YearsDescriptionRowCell.Y] = string.Format("На  {0} / {1} учебный год", year.Year, Convert.ToInt32(year.Year) + 1);
            ObjWorkSheet.Cells[Properties.IndPlanExport.Default.TeacherFIORowCell.X,
             Properties.IndPlanExport.Default.TeacherFIORowCell.Y] = employee.Name;
            PrintSemester(employee, SemesterType.Autumm, year, ObjWorkBook);//осенний семестр
            PrintSemester(employee, SemesterType.Spring, year, ObjWorkBook);//весенний семестр
            ObjExcel.Visible = true;
            ObjExcel.UserControl = true;
        }
        private static void PrintSemester(Employee employee, SemesterType semesterType, StudyYear year, Workbook objWorkBook)
        {
            int descrList = semesterType == SemesterType.Autumm ? 2 : 3;
            int paramsList = semesterType == SemesterType.Autumm ? 4 : 5;
            List<DisciplineWorkload> workloads = semesterType == SemesterType.Autumm ?
                _service.GetAllEmloyeeWorkloadsByYearAutumm(employee, year.Year) : _service.GetAllEmloyeeWorkloadsByYearSpring(employee, year.Year);
            workloads = workloads.OrderBy(w => w.Semester.Number).ToList();
            if (workloads.Count > 0)
            {
                Worksheet DescrSheet = (Worksheet)objWorkBook.Sheets[descrList];
                Worksheet ParamsSheet = (Worksheet)objWorkBook.Sheets[paramsList];
                int currentRow = IndPlanExport.Default.DisciplinesStartRow;
                foreach (var w in workloads)
                {
                    DescrSheet.Cells[currentRow, IndPlanExport.Default.DisciplineNameColumn] = w.DisciplineYear.Discipline.ToString();
                    if (w.DisciplineYear.Discipline.SpecialType == SpecialDisciplineKind.RUK_KAF)
                    {
                        ParamsSheet.Cells[currentRow - 1, IndPlanExport.Default.DisciplineSettingsStartColumn + 14] = CalculationSettings.Default.RukKaf;
                        currentRow += 2;
                        continue;
                    }
                    int cource = year.Year - w.Group.EntryYear + 1;
                    string descr = string.Format("{0},{1},{2} курс", w.Group.Speciality.Faculty, w.Group.Speciality, cource);
                    DescrSheet.Cells[currentRow, IndPlanExport.Default.DisciplineDescrColumn] = descr;
                    DescrSheet.Cells[currentRow, IndPlanExport.Default.StudentsCountColumn] = w.Group.CountOfStudents;
                    int weeks = w.Semester.CountOfWeeks;
                    int countStud = w.Group.CountOfStudents;
                    float lecFact = weeks * w.DisciplineYear.CountOfLecture * Properties.CalculationSettings.Default.LectureCost;
                    float pracFact = weeks * w.DisciplineYear.CountOfPractice * Properties.CalculationSettings.Default.PracticeCost;
                    float labFact = weeks * w.DisciplineYear.CountOfLabs * Properties.CalculationSettings.Default.LabCost;
                    float ekzFact = w.DisciplineYear.HasEx ? Properties.CalculationSettings.Default.ExamControlCost * countStud : 0;
                    //float konsFact = Convert.ToBoolean(reader[14]) ? Properties.CalculationSettings.Default.KonsCost * Convert.ToInt32(reader[6]) * weeks  + (ekzFact!=0?2:0) : 0;\
                    float konsFact = Properties.CalculationSettings.Default.KonsCost * w.DisciplineYear.CountOfLecture * weeks + (ekzFact != 0 ? 2 : 0);
                    float zachFact = w.DisciplineYear.HasCR ? Properties.CalculationSettings.Default.ZachCost * countStud : 0;
                    float KPFact = w.DisciplineYear.HasKP ? Properties.CalculationSettings.Default.KPCost * countStud : 0;
                    float KRFact = w.DisciplineYear.HasKR ? Properties.CalculationSettings.Default.KRCost * countStud : 0;
                    ParamsSheet.Cells[currentRow - 1, Properties.IndPlanExport.Default.DisciplineSettingsStartColumn] = lecFact;
                    ParamsSheet.Cells[currentRow - 1, Properties.IndPlanExport.Default.DisciplineSettingsStartColumn + 1] = pracFact;
                    ParamsSheet.Cells[currentRow - 1, Properties.IndPlanExport.Default.DisciplineSettingsStartColumn + 2] = labFact;
                    ParamsSheet.Cells[currentRow - 1, Properties.IndPlanExport.Default.DisciplineSettingsStartColumn + 3] = konsFact;
                    ParamsSheet.Cells[currentRow - 1, Properties.IndPlanExport.Default.DisciplineSettingsStartColumn + 4] = ekzFact;
                    ParamsSheet.Cells[currentRow - 1, Properties.IndPlanExport.Default.DisciplineSettingsStartColumn + 5] = zachFact;
                    ParamsSheet.Cells[currentRow - 1, Properties.IndPlanExport.Default.DisciplineSettingsStartColumn + 6] = KPFact;
                    ParamsSheet.Cells[currentRow - 1, Properties.IndPlanExport.Default.DisciplineSettingsStartColumn + 7] = KRFact;
                    double other = 0;
                    double geks = 0;
                    double ruk = 0;
                    double uchPr = w.DisciplineYear.CountOfLearnigPracticeWeeks * Properties.CalculationSettings.Default.UchPr;
                    double prPr = w.DisciplineYear.CountOfManufacturePracticeWeeks * Properties.CalculationSettings.Default.PrPr;
                    double preddipPr = w.DisciplineYear.CountOfUndergraduatePracticeWeeks * Properties.CalculationSettings.Default.PreddipPr;
                    double NIIR = w.DisciplineYear.CountOfNIIR * Properties.CalculationSettings.Default.NIIR * countStud;//нир
                    double GEK = w.DisciplineYear.Discipline.SpecialType == SpecialDisciplineKind.GEK ? (Properties.CalculationSettings.Default.GEK * countStud) : 0;//ГЭК.
                    double GAKpred = w.DisciplineYear.Discipline.SpecialType == SpecialDisciplineKind.GAK_PRED ? (Properties.CalculationSettings.Default.GEK * countStud) : 0;
                    double GAK = w.DisciplineYear.Discipline.SpecialType == SpecialDisciplineKind.GAK ? (Properties.CalculationSettings.Default.GAK * countStud) : 0;//ГAК
                    double rukMag = w.DisciplineYear.Discipline.SpecialType == SpecialDisciplineKind.MAG_RUK ? (Properties.CalculationSettings.Default.MAGRuk * countStud) : 0;//рук маг
                    geks = GEK + GAK + GAKpred;
                    ruk += w.DisciplineYear.Discipline.SpecialType == SpecialDisciplineKind.ASP_RUK ? (Properties.CalculationSettings.Default.AspRuk * countStud) : 0f;
                    ruk += rukMag;
                    ruk += w.DisciplineYear.Discipline.SpecialType == SpecialDisciplineKind.BAK_RUK ? (countStud * Properties.CalculationSettings.Default.DPruk) : 0f;
                    ruk += w.DisciplineYear.Discipline.SpecialType == SpecialDisciplineKind.MAG_RETZ ? countStud * CalculationSettings.Default.MagRetz : 0f;//MagRetz
                    ruk += w.DisciplineYear.Discipline.SpecialType == SpecialDisciplineKind.RUK_KAF ? countStud : 0f;//руководство кафедрой
                    other += NIIR;
                    /*string disciplineName = reader[0].ToString();
                    if (disciplineName.ToLower().Contains("норм") && disciplineName.ToLower().Contains("маг"))
                        other += countStud * Properties.CalculationSettings.Default.NormocontrolMag;
                    if (disciplineName.ToLower().Contains("доп") && disciplineName.ToLower().Contains("маг"))
                        other += countStud * Properties.CalculationSettings.Default.DopuskDissMag;*/
                    ParamsSheet.Cells[currentRow - 1, Properties.IndPlanExport.Default.DisciplineSettingsStartColumn + 8] = 0;
                    ParamsSheet.Cells[currentRow - 1, Properties.IndPlanExport.Default.DisciplineSettingsStartColumn + 9] = uchPr;//уч пр.
                    ParamsSheet.Cells[currentRow - 1, Properties.IndPlanExport.Default.DisciplineSettingsStartColumn + 10] = prPr;//пр пр.
                    ParamsSheet.Cells[currentRow - 1, Properties.IndPlanExport.Default.DisciplineSettingsStartColumn + 11] = preddipPr;//преддип пр.
                    ParamsSheet.Cells[currentRow - 1, Properties.IndPlanExport.Default.DisciplineSettingsStartColumn + 12] = 0;
                    ParamsSheet.Cells[currentRow - 1, Properties.IndPlanExport.Default.DisciplineSettingsStartColumn + 13] = geks;
                    ParamsSheet.Cells[currentRow - 1, Properties.IndPlanExport.Default.DisciplineSettingsStartColumn + 14] = ruk;
                    ParamsSheet.Cells[currentRow - 1, Properties.IndPlanExport.Default.DisciplineSettingsStartColumn + 15] = other;
                    currentRow += 2;
                }
            }
        }
        public static void ExportSemester(StudyYear year, SemesterType semester)
        {

            string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Templates\SemesterTemplate.xltx");
            Dictionary<string, int> counts = new Dictionary<string, int>() { { "ФИТ", 0 }, { "МСФ", 0 }, { "ИДПО", 0 }, { "МАГ", 0 } };
            List<DisciplineWorkload> workloads = _service.GetAllDisciplineWorkloadsByYearAndSemesterType(year.Year, semester);
            workloads = workloads.OrderBy(w => w.Semester.Number).ToList();
            if (workloads.Count > 0)
            {
                Microsoft.Office.Interop.Excel.Application ObjExcel = new Microsoft.Office.Interop.Excel.Application();
                Workbook ObjWorkBook;
                Worksheet ObjWorkSheet;
                ObjWorkBook = ObjExcel.Workbooks.Add(path);
                ObjWorkSheet = (Worksheet)ObjWorkBook.Sheets[1];
                ObjWorkBook.Title = string.Format("Отчет {0} семестр({1} / {2})", semester.ToDescriptionString(), year.Year, year.Year + 1);
                ObjWorkSheet.Cells[3, 1] = string.Format("экзаменационной сессии  за  {0} семестр {1} / {2} учебного года", semester.ToDescriptionString(), year.Year, year.Year + 1);
                foreach (var w in workloads)
                {
                    int rowCounter = SemesterExport.Default.FITStartRow;
                    if (w.DisciplineYear.Discipline.TypeOfDiscipline == DisciplineType.SPECIAL)
                    {
                        rowCounter++;
                        continue;
                    }
                    Group group = w.Group;
                    int cource = year.Year - group.EntryYear + 1;
                    Discipline discipline = w.DisciplineYear.Discipline;
                    int countStud = group.CountOfStudents;
                    int weeks = w.Semester.CountOfWeeks;
                    int lec = w.DisciplineYear.CountOfLecture;
                    int prac = w.DisciplineYear.CountOfPractice;
                    int lab = w.DisciplineYear.CountOfLabs;
                    bool kr = w.DisciplineYear.HasKR;
                    bool kp = w.DisciplineYear.HasKP;
                    bool ekz = w.DisciplineYear.HasEx;
                    bool zach = w.DisciplineYear.HasCR;

                    float summ = WorkloadsCalculator.GetWorkloadCost(w);
                    int index = 0;
                    if (w.Group.StudyForm != StudyForm.FullTime)
                    {
                        rowCounter = counts["ФИТ"] + counts["ИДПО"] + counts["МСФ"] + SemesterExport.Default.IDPOStartRow;
                        counts["ИДПО"]++;
                        index = counts["ИДПО"];
                    }
                    else
                    if (w.Group.Speciality.Faculty.ShortName == "ФИТ")
                    {
                        if (w.Group.Qualification != Qualification.Magistracy)
                        {
                            rowCounter += counts["ФИТ"];
                            counts["ФИТ"]++;
                            index = counts["ФИТ"];
                        }
                        else
                        {
                            rowCounter = counts["ФИТ"] + counts["ИДПО"] + counts["МАГ"] + counts["МСФ"] + SemesterExport.Default.MAGStartRow;
                            counts["МАГ"]++;
                            index = counts["МАГ"];
                        }
                    }
                    else
                    if (w.Group.Speciality.Faculty.ShortName == "МСФ")
                    {
                        rowCounter = counts["ФИТ"] + counts["МСФ"] + SemesterExport.Default.MSFStartRow;
                        counts["МСФ"]++;
                        index = counts["МСФ"];
                    }
                    {
                        Range line = (Range)ObjWorkSheet.Rows[rowCounter];
                        line.Insert();

                        ObjWorkSheet.Cells[rowCounter, SemesterExport.Default.IndexColumn] = index;
                        ObjWorkSheet.Cells[rowCounter, SemesterExport.Default.GroupColumn] = group.Speciality.Name;
                        ObjWorkSheet.Cells[rowCounter, SemesterExport.Default.CourceColumn] = cource;
                        ObjWorkSheet.Cells[rowCounter, SemesterExport.Default.DisciplineColumn] = w.DisciplineYear.Discipline.Name;
                        ObjWorkSheet.Cells[rowCounter, SemesterExport.Default.DisciplineCostColumn] = summ;
                        ObjWorkSheet.Cells[rowCounter, SemesterExport.Default.LecFactColumn] = lec * weeks;
                        ObjWorkSheet.Cells[rowCounter, SemesterExport.Default.StudentsColumn] = countStud;
                        ObjWorkSheet.Cells[rowCounter, SemesterExport.Default.DisciplineColumn].EntireRow.AutoFit(); //применить автовысоту
                    }
                    rowCounter++;
                }

                ObjExcel.Visible = true;
                ObjExcel.UserControl = true;
            }
        }
        public static void ExportWorkload(StudyYear year)
        {
            string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Templates\WorkloadTemplate.xltx");
            Microsoft.Office.Interop.Excel.Application ObjExcel = new Microsoft.Office.Interop.Excel.Application();
            Workbook ObjWorkBook;
            Worksheet ObjWorkSheet;
            ObjWorkBook = ObjExcel.Workbooks.Add(path);//System.Reflection.Missing.Value);
            ObjWorkSheet = (Worksheet)ObjWorkBook.Sheets[1];
            ObjWorkBook.Title = string.Format("Нагрузка ({0} / {1})", year.Year, year.Year + 1);
            //

            for (int i = 1; i < Math.Min(ObjWorkBook.Sheets.Count, 18); i++)
            {
                ObjWorkBook.Sheets[i].Cells[1, 17] = string.Format("{0} / {1}", year.Year, year.Year + 1);
            }
            List<DisciplineWorkload> dis_workloads = _service.GetAllDisciplineWorkloadsByYear(year).OrderBy(s=>s.Semester.Number).ToList();          
            Dictionary<Worksheet, int> rowCounters = new Dictionary<Worksheet, int>();
            //пишем данные
            for (int i = 2; i <= 17; i++)
            {
                rowCounters.Add(ObjWorkBook.Sheets[i], 6);
            }

            foreach (var d in dis_workloads)
            {
                List<Worksheet> toWriteList = new List<Worksheet>();
                Dictionary<Worksheet, Workload> assignsForTeachers = new Dictionary<Worksheet, Workload>();
                if (d.DisciplineYear.Discipline.SpecialType == SpecialDisciplineKind.RUK_KAF)
                {
                    var buf = _service.GetAllWorkloadsByLocalWorkload(d.Id);
                    if (buf.Count == 1)
                    {                
                        string teacherName = buf[0].Employee?.Name;
                        var sheet = ObjWorkBook.Sheets[teacherName];                       
                        toWriteList.Add(sheet);
                        assignsForTeachers.Add(sheet, buf[0]);                      
                    }
                continue;
                }
                if (d.Group.Speciality.Faculty.ShortName == "МСФ")
                    toWriteList.Add(ObjWorkBook.Sheets[6]);
                else
                {
                    if (d.Group.StudyForm == StudyForm.PartTime)
                        toWriteList.Add(ObjWorkBook.Sheets[5]);
                    else
                    {
                        if (d.Group.Qualification == Qualification.Bachelor)
                            toWriteList.Add(ObjWorkBook.Sheets[4]);
                        else
                            toWriteList.Add(ObjWorkBook.Sheets[8]);
                    }
                }
                List<Workload> assigns = _service.GetAllWorkloadsByLocalWorkload(d.Id);
                if (assigns.Count != 0)
                {
                    foreach (var assign in assigns)
                    {
                        string teacherName = assign.Employee?.Name;
                        if (teacherName != null)
                        {
                            Worksheet sheet = null;
                            try
                            {
                                sheet = ObjWorkBook.Sheets[teacherName];
                            }
                            catch
                            {
                                throw new Exception($"В книге нет листа для преподавателя '{teacherName}' ");
                            }
                            if (sheet != null)
                            {
                                toWriteList.Add(sheet);
                                assignsForTeachers.Add(sheet, assign);
                            }
                        }
                    }
                }
                else
                {
                    if (d.Group.Qualification == Qualification.Bachelor)
                    {
                        toWriteList.Add(ObjWorkBook.Sheets[2]);
                    }
                    else
                    {
                        toWriteList.Add(ObjWorkBook.Sheets[3]);
                    }
                }
                foreach (Worksheet sheet in toWriteList)
                {
                    int countStud = d.Group.CountOfStudents;
                    /*if (sheet.Index > 8 && Convert.ToBoolean(reader[34]))
                    {
                        Workload assign = DataManager.SharedDataManager().GetWorkloadAssign(Convert.ToInt32(reader[1]), sheet.Name);
                        countStud = assign.StudentsCount;
                    }*/
                    ExportNotAssign(d, sheet, rowCounters, countStud);
                }
            }
            ObjExcel.Visible = true;
            ObjExcel.UserControl = true;
        }
        private static void ExportNotAssign(DisciplineWorkload disciplineWorkload, Worksheet sheet, Dictionary<Worksheet, int> rowCounters, int countStud)
        {
            sheet.Cells[rowCounters[sheet], 1] = rowCounters[sheet] - 5;
            sheet.Cells[rowCounters[sheet], 2] = disciplineWorkload.DisciplineYear.Discipline.Name;
            sheet.Cells[rowCounters[sheet], 3] = disciplineWorkload.Group.Speciality.Name;
            sheet.Cells[rowCounters[sheet], 5] = disciplineWorkload.Group.StudyForm == StudyForm.FullTime ? "о" : "з";
            sheet.Cells[rowCounters[sheet], 4] = disciplineWorkload.Group.Speciality.Faculty.ShortName;
            sheet.Cells[rowCounters[sheet], 6] = disciplineWorkload.Semester.Number;
            sheet.Cells[rowCounters[sheet], 7] = countStud;
            sheet.Cells[rowCounters[sheet], 8] = disciplineWorkload.Semester.CountOfWeeks;
            sheet.Cells[rowCounters[sheet], 11] = disciplineWorkload.DisciplineYear.CountOfLecture;
            sheet.Cells[rowCounters[sheet], 12] = disciplineWorkload.DisciplineYear.CountOfPractice;
            sheet.Cells[rowCounters[sheet], 13] = disciplineWorkload.DisciplineYear.CountOfLabs;
            sheet.Cells[rowCounters[sheet], 14] = disciplineWorkload.DisciplineYear.HasEx ? "1" : "";
            sheet.Cells[rowCounters[sheet], 15] = disciplineWorkload.DisciplineYear.HasCR ? "+" : "";
            //sheet.Cells[rowCounters[sheet], 16] = Convert.ToBoolean(reader[15]) ? "1" : "";
            sheet.Cells[rowCounters[sheet], 17] = disciplineWorkload.DisciplineYear.HasKR ? "1" : "";
            sheet.Cells[rowCounters[sheet], 18] = disciplineWorkload.DisciplineYear.HasKP ? "1" : "";
            //sheet.Cells[rowCounters[sheet], 22] = Convert.ToBoolean(reader[33]) ? "1" : "";//нир
            sheet.Cells[rowCounters[sheet], 19 + 23] = disciplineWorkload.DisciplineYear.CountOfLearnigPracticeWeeks != 0 ? disciplineWorkload.DisciplineYear.CountOfLearnigPracticeWeeks * CalculationSettings.Default.UchPr : 0;//уч пр.
            sheet.Cells[rowCounters[sheet], 20 + 23] = disciplineWorkload.DisciplineYear.CountOfManufacturePracticeWeeks != 0 ? disciplineWorkload.DisciplineYear.CountOfManufacturePracticeWeeks * CalculationSettings.Default.PrPr : 0;//пр пр.
            sheet.Cells[rowCounters[sheet], 21 + 23] = disciplineWorkload.DisciplineYear.CountOfUndergraduatePracticeWeeks != 0 ? disciplineWorkload.DisciplineYear.CountOfUndergraduatePracticeWeeks * CalculationSettings.Default.PreddipPr : 0;//преддип пр.
            sheet.Cells[rowCounters[sheet], 22 + 23] = disciplineWorkload.DisciplineYear.CountOfNIIR != 0 ? (disciplineWorkload.DisciplineYear.CountOfNIIR) * CalculationSettings.Default.NIIR * countStud : 0;//нир
            ((Range)(sheet.Cells[rowCounters[sheet], 24 + 23])).Value = disciplineWorkload.DisciplineYear.Discipline.SpecialType == SpecialDisciplineKind.GEK ? (CalculationSettings.Default.GEK * countStud) : 0;//ГЭК.
            sheet.Cells[rowCounters[sheet], 25 + 23] = disciplineWorkload.DisciplineYear.Discipline.SpecialType == SpecialDisciplineKind.GAK ? (CalculationSettings.Default.GAK * countStud) : 0;//ГAК.
            sheet.Cells[rowCounters[sheet], 26 + 23] = disciplineWorkload.DisciplineYear.Discipline.SpecialType == SpecialDisciplineKind.GAK_PRED ? (CalculationSettings.Default.GAKPred * countStud) : 0;//ГAКпред.
            sheet.Cells[rowCounters[sheet], 27 + 23] = disciplineWorkload.DisciplineYear.Discipline.SpecialType == SpecialDisciplineKind.BAK_RUK ? (CalculationSettings.Default.DPruk * countStud) : 0;
            sheet.Cells[rowCounters[sheet], 28 + 23] = disciplineWorkload.DisciplineYear.Discipline.SpecialType == SpecialDisciplineKind.MAG_RETZ ? (CalculationSettings.Default.MagRetz * countStud).ToString() : "";//рец дисс.
            /*string disciplineName = reader[4].ToString();
            if (disciplineName.ToLower().Contains("норм") && disciplineName.ToLower().Contains("маг"))
                sheet.Cells[rowCounters[sheet], 32 + 23] = countStud * ApplicationSettings.CalculationSettings.NormocontrolMag;
            if (disciplineName.ToLower().Contains("доп") && disciplineName.ToLower().Contains("маг"))
                sheet.Cells[rowCounters[sheet], 32 + 23] = countStud * ApplicationSettings.CalculationSettings.DopuskDissMag;*/
            sheet.Cells[rowCounters[sheet], 32 + 23] = disciplineWorkload.DisciplineYear.Discipline.SpecialType == SpecialDisciplineKind.MAG_RUK ? (CalculationSettings.Default.MAGRuk * countStud).ToString() : "";//рук маг
            //sheet.Cells[rowCounters[sheet], 31 + 23] = Convert.ToBoolean(reader[29]) ? (countStud * ApplicationSettings.CalculationSettings.AspRuk) : 0f;
            /*if (disciplineName.ToLower().Contains("норм") && disciplineName.ToLower().Contains("маг"))
                sheet.Cells[rowCounters[sheet], 32 + 23] = countStud * ApplicationSettings.CalculationSettings.NormocontrolMag;
            if (disciplineName.ToLower().Contains("доп") && disciplineName.ToLower().Contains("маг"))
                sheet.Cells[rowCounters[sheet], 32 + 23] = countStud * ApplicationSettings.CalculationSettings.DopuskDissMag;*/
            rowCounters[sheet]++;
        }
    }
}
