using Diploma.Controls.Managers;
using Diploma.Screens.Managers;
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

namespace Diploma.Dialogs.Managers
{   

    public class AddWorkloadManager : BaseManager
    {
        public Action OnExit { get; set; }
        public string Title { get; set; }
        private Workload _workload;
        private StudyYear _year;
        public Workload Workload { get { return _workload; } set { _workload = value;RaisePropertyChanged();} }
        public ObservableCollection<DisciplineWorkload> LocalWorkloads{ get; set; }
        public ObservableCollection<Employee> Employees { get; set; }
 

        public AddWorkloadManager(StudyYear year)
        {
            Workload = new Workload();
            Title = "Добавить дисциплину";
            Initialize(year);
        }
        public AddWorkloadManager(Workload workload, StudyYear year)
        {
            Workload = workload;
            Title = "Изменить дисциплину";
            Initialize(year);
        }
        private void Initialize(StudyYear year)
        {
            _year = year;
            Panel = new PanelManager
            {
                RightButtons = new List<PanelButtonManager>
                {
                    new PanelButtonManager{OnButtonAction = o=> Save(),Icon = PackIconModernKind.Save,Text = "Save"}
                }
            }; 
        }

        private void Save()
        {          
            var service = Get<IGeneralService>();
            service.AddOrUpdateWorkload(_workload);
            OnExit();
        }

        public override async void Refresh()
        {
            var service = Get<IGeneralService>();
            Employees = new ObservableCollection<Employee>(service.GetAllEmployees());
            LocalWorkloads = new ObservableCollection<DisciplineWorkload>(service.GetAllDisciplineWorkloadsByYear(_year));
            
            RaisePropertyChanged("Employees");
            RaisePropertyChanged("LocalWorkloads");

        }
    }
}
