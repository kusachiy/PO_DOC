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

    public class CalculateSettingsManager : BaseManager
    {
        public Action OnExit { get; set; }

        public ObservableCollection<Semester> Semesters { get; set; }

        private Semester _selectedSemester;
        public Semester SelectedSemester { get { return _selectedSemester; } set { _selectedSemester = value;RaisePropertyChanged(); } }
        public CalculateSettingsManager()
        {
            Panel = new PanelManager
            {
                LeftButtons = new List<PanelButtonManager>
                {
                    new PanelButtonManager{OnButtonAction = o=> Add(),Icon = PackIconModernKind.Add,Text = "Add"},                   
                },               

                RightButtons = new List<PanelButtonManager>
                {
                    new PanelButtonManager{OnButtonAction = o=> Save(),Icon = PackIconModernKind.Save,Text = "Save"},
                    new PanelButtonManager{OnButtonAction =o=>Delete(),Icon = PackIconModernKind.Delete,Text="DeleteRow"}
                }
            };
        }

        private async void Delete()
        {
            if (SelectedSemester == null)
                return;            
            var service = Get<IGeneralService>();
            service.DeleteSemester(SelectedSemester);
            Refresh();
        }

        private void Add()
        {
            int number = Semesters.Count > 0 ? Semesters.Last().Number + 1 : 1;
            var sem = new Semester {Number = number };
            Semesters.Add(sem);
            SelectedSemester = sem;
            RaisePropertyChanged("Semesters");            
        }

        private void Save()
        {
            var service = Get<IGeneralService>();
            foreach (var sem in Semesters)
                service.AddOrUpdateSemester(sem);
            OnExit();
        }

        public override async void Refresh()
        {
            var service = Get<IGeneralService>();
            Semesters = new ObservableCollection<Semester>(await Task.Run(() => service.GetAllSemesters().OrderBy(s=>s.Number)));
            RaisePropertyChanged("Semesters");
        }
    }
}
