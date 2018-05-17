using Diploma.Controls.Managers;
using Diploma.Services;
using Diploma.Utils;
using MahApps.Metro.IconPacks;
using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Screens.Managers
{
    class DisciplineWorkloadManager : BaseManager
    {
        public List<DisciplineWorkload> Workloads { get; set; }
        private DisciplineWorkload _selectedWorkload;
        public DisciplineWorkload Selectedworkload { get { return _selectedWorkload; } set { _selectedWorkload = value; RaisePropertyChanged(); } }
        private StudyYear _selectedStudyYear;
        public StudyYear SelectedStudyYear
        {
            get { return _selectedStudyYear; }
            set
            {
                if (_selectedStudyYear == value)
                    return;
                _selectedStudyYear = value;
                RaisePropertyChanged();   
                RefreshTable();

            }
        }
        private Semester _selectedSemester;
        public Semester SelectedSemester
        {
            get { return _selectedSemester; }
            set
            {
                if (_selectedSemester == value)
                    return;
                _selectedSemester = value;
                RaisePropertyChanged();
                RefreshTable();
            }
        }
        public ObservableCollection<StudyYear> StudyYears { get; set; }
        public ObservableCollection<Semester> Semesters { get; set; }

        public DisciplineWorkloadManager()
        {
            Panel = new PanelManager
            {
                MiddleButtons = new List<PanelButtonManager>
                {
                    new PanelButtonManager{OnButtonAction = o=> Refresh(),Icon = PackIconModernKind.Refresh,Text = "Refresh"}

                },
                RightButtons = new List<PanelButtonManager>
                {
                    new PanelButtonManager{OnButtonAction = o=> SaveHandler(),Icon = PackIconModernKind.Save,Text = "Save"}
                }
            };
        }

        private async void SaveHandler()
        {
            SetWaiting(true);
            await Task.Run(()=>Save());
            await DialogHelper.ShowMessageDialog("Сохранено","");
            SetWaiting(false);
        }

        private void Save()
        {            
            
        }

        public override async void Refresh()
        {
            SetWaiting(true);
            var service = Get<IGeneralService>();
            Semesters = new ObservableCollection<Semester>(await Task.Run(() => service.GetAllSemesters().OrderBy(y=>y.Number)));
            StudyYears = new ObservableCollection<StudyYear>(await Task.Run(() => service.GetAllStudyYears().OrderBy(s => s.Year)));
            RaisePropertyChanged("StudyYears");
            RaisePropertyChanged("Semesters");
            SetWaiting(false);
            RefreshTable();
        }

        private void RefreshTable()
        {
            SetWaiting(true);
            var service = Get<IGeneralService>();
            if (_selectedSemester == null || _selectedStudyYear == null)
            {
                SetWaiting(false);
                return;
            }
            Workloads = service.GetAllDisciplineWorkloadsByYearAndSemester(_selectedStudyYear, _selectedSemester);
            RaisePropertyChanged("Workloads");
            SetWaiting(false);
        }
    }
}
