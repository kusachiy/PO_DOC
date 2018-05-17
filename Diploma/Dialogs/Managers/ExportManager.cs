using Diploma.Controls.Managers;
using Diploma.Screens.Managers;
using Diploma.Services;
using Diploma.Utils.ExcelHelpers;
using GalaSoft.MvvmLight.Command;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.IconPacks;
using Microsoft.Win32;
using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace Diploma.Dialogs.Managers
{

    public class ExportManager : BaseManager
    {
        private int _selectedType;


        public Action OnExit { get; set; }
        public string[] ReportTypes { get; set; }
        public string[] Semesters { get; set; }
        public int SelectedType { get { return _selectedType; } set { if (_selectedType == value) return;  _selectedType = value;TypeChanged(); } }     

        public int SelectedSemester { get; set; }
        public string Log { get; set; }     
        public RelayCommand StartCommand { get; set; }
        public ObservableCollection<StudyYear> Years { get; set; }
        public ObservableCollection<Employee> Employees { get; set; }
        public StudyYear SelectedYear { get; set; }
        public Employee SelectedEmployee { get; set; }


        public Visibility SemesterVisibility { get; set; }
        public Visibility EmployeesVisibility { get; set; }


        public ExportManager()
        {
            ReportTypes = new string[] { "Индивидуальный план", "Семестр", "Нагрузка" };
            SelectedType = -1;
            SelectedSemester = -1;
            Semesters = new string[] { "Осенний", "Весенний" };
            Panel = new PanelManager
            {
                RightButtons =  new List<PanelButtonManager>
                {
                    new PanelButtonManager{OnButtonAction= o=>Start(),Icon=PackIconModernKind.OfficeExcel,Text = "Экспорт"}
                }
            };
            StartCommand = new RelayCommand(Start);
        }
        private void TypeChanged()
        {
            EmployeesVisibility = Visibility.Collapsed;
            SemesterVisibility = Visibility.Collapsed;
            if (_selectedType == 0)
                EmployeesVisibility = Visibility.Visible;
            if (_selectedType == 1)
                SemesterVisibility = Visibility.Visible;
            RaisePropertyChanged("EmployeesVisibility");
            RaisePropertyChanged("SemesterVisibility");
        }       

        private async void Start()
        {
            if (_selectedType == -1 || SelectedYear == null)
                return;
            if (_selectedType == 0)
            {
                if (SelectedEmployee == null)
                    return;
                Waiting = true;
                await Task.Run(()=>ExcelExporter.ExportIndividualPlan(SelectedEmployee, SelectedYear));
                Waiting = false;
                return;
            }
            if (_selectedType == 1)
            {
                if (SelectedSemester == -1)
                    return;
                Waiting = true;
                await Task.Run(() => ExcelExporter.ExportSemester(SelectedYear,(SemesterType)SelectedSemester));
                Waiting = false;
                return;
            }
            if (_selectedType == 2)
            {
                Waiting = true;
                await Task.Run(() => ExcelExporter.ExportWorkload(SelectedYear));
                Waiting = false;
                return;
            }
        }    
        
        
        public override async void Refresh()
        {
            var service = Get<IGeneralService>();
            Years = new ObservableCollection<StudyYear>(await Task.Run(()=> service.GetAllStudyYears()));
            Employees = new ObservableCollection<Employee>(await Task.Run(() => service.GetAllEmployees()));
            RaisePropertyChanged("Years");
            RaisePropertyChanged("Employees");
        }
    }
}
