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
    class DisciplinesManager:BaseManager
    {
        private Discipline _selectedDiscipline;
        public Discipline SelectedDiscipline { get { return _selectedDiscipline; } set { _selectedDiscipline = value; RaisePropertyChanged(); } }
        public List<Discipline> Disciplines { get; set; }
        public DisciplinesManager()
        {
            Panel = new PanelManager
            {
                LeftButtons = new List<PanelButtonManager>
                {
                    new PanelButtonManager{OnButtonAction = o=> Add(),Icon = PackIconModernKind.Add,Text = "Add"},
                    new PanelButtonManager{OnButtonAction = o=> Edit(),Icon = PackIconModernKind.Edit,Text = "Edit"}
                },
                RightButtons = new List<PanelButtonManager>
                {
                    new PanelButtonManager{OnButtonAction = o=> Delete(),Icon = PackIconModernKind.Delete,Text = "Delete"},
                }
            };
        }
        private void Add()
        {
            SetWaiting(true);
            var manager = new AddDisciplineManager { SetWaiting = isBusy => SetWaiting(isBusy) };
            var dialog = new AddDisciplineScreen(manager);
            dialog.Closed += async (sender, args) =>
            {
                Refresh();
            };
            dialog.Show();
        }
        private void Edit()
        {
            if (SelectedDiscipline == null)
                return;
            SetWaiting(true);
            var addPaymentManager = new AddDisciplineManager(SelectedDiscipline);
            addPaymentManager.SetWaiting = isBusy => SetWaiting(isBusy);
            var addPaymentDialog = new AddDisciplineScreen(addPaymentManager);
            addPaymentDialog.Closed += async (sender, args) =>
            {
                Refresh();
            };
            addPaymentDialog.Show();
        }
        private async void Delete()
        {
            if (SelectedDiscipline == null)
                return;
            bool res = await DialogHelper.ShowAffirmationDialog("Подтвердите удаление объекта", "Вы уверен что хотите удалить данный объект? Данное действие необратимо");
            if (!res)
                return;
            var service = Get<IGeneralService>();
            service.DeleteDiscipline(SelectedDiscipline);
            Refresh();
        }

        public override void Refresh()
        {
            SetWaiting(true);
            var service = Get<IGeneralService>();
            Disciplines = service.GetAllDisciplines();
            RaisePropertyChanged("Disciplines");
            SetWaiting(false);
        }
    }
}
