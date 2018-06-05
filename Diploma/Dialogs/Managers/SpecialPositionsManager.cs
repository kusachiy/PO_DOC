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

        public ObservableCollection<Discipline> SpecialDisciplines { get; set; }
        private SpecialPosition _selectedPosition;
        public SpecialPosition SelectedDiscipline { get { return _selectedPosition; } set { _selectedPosition = value;RaisePropertyChanged(); } }
        public SpecialPositionsManager()
        {
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
            foreach (var sd in SpecialDisciplines)
                service.AddOrUpdateDiscipline(sd);
            OnExit();
        }

        public override async void Refresh()
        {
            var service = Get<IGeneralService>();
            SpecialDisciplines = new ObservableCollection<Discipline>(await Task.Run(() => service.GetAllSpecialDisciplines()));
            RaisePropertyChanged("SpecialDisciplines");  
        }
    }
}
