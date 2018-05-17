using Diploma.Controls.Managers;
using Diploma.Dialogs;
using Diploma.Dialogs.Managers;
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
    class WorkloadManager : BaseManager
    {
        public List<Workload> Workloads { get; set; }
        private Workload _selectedWorkload;
        public Workload SelectedWorkload { get { return _selectedWorkload; } set { _selectedWorkload = value; RaisePropertyChanged(); } }
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
        public ObservableCollection<Employee> Employees { get; set; }
        public ObservableCollection<StudyYear> StudyYears { get; set; }


        public WorkloadManager()
        {
            Panel = new PanelManager
            {
                LeftButtons = new List<PanelButtonManager>
                {
                    new PanelButtonManager{OnButtonAction = o=>AddNewWorkloadHandler(),Icon = PackIconModernKind.Add,Text = "Добавить нагрузку" },
                    new PanelButtonManager{OnButtonAction = o=>RemoveWorkloadHandler(), Icon = PackIconModernKind.Delete,Text = "Удалить"}
                },
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

        private async void RemoveWorkloadHandler()
        {
            if (SelectedWorkload == null)
                return;            
            SetWaiting(true);
            await Task.Run(() => Delete(SelectedWorkload));
            await DialogHelper.ShowMessageDialog("Удалено", "");
            RefreshTable();
            SetWaiting(false);
        }       

        private void AddNewWorkloadHandler()
        {
            SetWaiting(true);
            var manager = new AddWorkloadManager(_selectedStudyYear) { SetWaiting = isBusy => SetWaiting(isBusy) };
            var dialog = new AddWorkloadScreen(manager);
            dialog.Closed += (sender, args) =>
            {
                RefreshTable();
            };
            dialog.Show();
        }

        private async void SaveHandler()
        {
            SetWaiting(true);
            await Task.Run(() => Save());
            await DialogHelper.ShowMessageDialog("Сохранено", "");
            SetWaiting(false);
        }

        private void Save()
        {
            var service = Get<IGeneralService>();
            foreach (var workload in Workloads)
            {
                service.UpdateWorkload(workload);
            };
        }
        private void Delete(Workload workload)
        {
            var service = Get<IGeneralService>();
            service.DeleteWorkload(workload);
        }

        public override async void Refresh()
        {
            SetWaiting(true);
            var service = Get<IGeneralService>();
            Employees = new ObservableCollection<Employee>(await Task.Run(() => service.GetAllEmployees().OrderBy(s => s.Name)));
            StudyYears = new ObservableCollection<StudyYear>(await Task.Run(() => service.GetAllStudyYears().OrderBy(s => s.Year)));
            RaisePropertyChanged("StudyYears");
            RaisePropertyChanged("Employees");
            SetWaiting(false);
            RefreshTable();
        }
        private async void RefreshTable()
        {
            Workloads = new List<Workload>();
            if (SelectedStudyYear == null)
            {
                SetWaiting(false);
                RaisePropertyChanged("Workloads");
                return;
            }
            SetWaiting(true);
            var service = Get<IGeneralService>();
            Workloads = await Task.Run(() => service.GetAllWorkloadsByYear(SelectedStudyYear));
            RaisePropertyChanged("Workloads");
            SetWaiting(false);

        }
    }
}
