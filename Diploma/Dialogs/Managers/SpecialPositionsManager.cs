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

    public class SpecialPositionsManager : BaseManager
    {
        public Action OnExit { get; set; }

        public ObservableCollection<SpecialPosition> SpecialPositions { get; set; }
        public ObservableCollection<Employee> Employees { get; set; }

        private SpecialPosition _selectedPosition;
        public SpecialPosition SelectedPosition { get { return _selectedPosition; } set { _selectedPosition = value;RaisePropertyChanged(); } }
        public SpecialPositionsManager()
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
            if (SelectedPosition == null)
                return;
            var service = Get<IGeneralService>();
            service.DeleteSpecialPosition(SelectedPosition);
            Refresh();
        }

        private void Add()
        {
            if (Employees.Count == 0)
                return;
            var pos = new SpecialPosition { Executor = Employees.FirstOrDefault() };
            SpecialPositions.Add(pos);
            SelectedPosition = pos;
            RaisePropertyChanged("SpecialPositions");
        }

        private void Save()
        {
            var service = Get<IGeneralService>();
            foreach (var sp in SpecialPositions)
                service.AddOrUpdateSpecialPosition(sp);
            OnExit();
        }

        public override async void Refresh()
        {
            var service = Get<IGeneralService>();
            SpecialPositions = new ObservableCollection<SpecialPosition>(await Task.Run(() => service.GetAllSpecialPostitions()));
            Employees = new ObservableCollection<Employee>(await Task.Run(() => service.GetAllEmployees().OrderBy(s=>s.Name)));
            RaisePropertyChanged("SpecialPositions");
            RaisePropertyChanged("Employees");
        }
    }
}
