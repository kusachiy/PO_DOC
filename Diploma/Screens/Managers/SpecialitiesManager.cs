using Diploma.Controls.Managers;
using Diploma.Dialogs;
using Diploma.Dialogs.Managers;
using Diploma.Services;
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
    class SpecialitiesManager:BaseManager
    {
        private Speciality _selectedSpeciality;
        public Speciality SelectedSpeciality { get { return _selectedSpeciality; } set { _selectedSpeciality = value; RaisePropertyChanged(); } }
        public List<Speciality> Specialities { get; set; }
        public SpecialitiesManager()
        {
            Panel = new PanelManager
            {
                LeftButtons = new List<PanelButtonManager>
                {
                    new PanelButtonManager{OnButtonAction = o=> Add(),Icon = PackIconModernKind.Add,Text = "Add"},
                    new PanelButtonManager{OnButtonAction = o=> Edit(),Icon = PackIconModernKind.Edit,Text = "Edit"}
                }
            };
        }
        private void Add()
        {
            SetWaiting(true);
            var manager = new AddSpecialityManager { SetWaiting = isBusy => SetWaiting(isBusy) };
            var dialog = new AddSpecialityScreen(manager);
            dialog.Closed += async (sender, args) =>
            {
                Refresh();
            };
            dialog.Show();
        }
        private void Edit()
        {
            if (SelectedSpeciality == null)
                return;
            SetWaiting(true);
            var addPaymentManager = new AddSpecialityManager(SelectedSpeciality);
            addPaymentManager.SetWaiting = isBusy => SetWaiting(isBusy);
            var addPaymentDialog = new AddSpecialityScreen(addPaymentManager);
            addPaymentDialog.Closed += async (sender, args) =>
            {
                Refresh();
            };
            addPaymentDialog.Show();
        }

        public override void Refresh()
        {
            SetWaiting(true);
            var service = Get<IGeneralService>();
            Specialities = service.GetAllSpecialities();
            RaisePropertyChanged("Specialities");
            SetWaiting(false);
        }
    }
}
