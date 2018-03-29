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
    class GroupsManager:BaseManager
    {
        private Group _selectedGroup;
        public Group SelectedGroup { get { return _selectedGroup; } set { _selectedGroup = value; RaisePropertyChanged(); } }
        public List<Group> Groups{ get; set; }
        public GroupsManager()
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
            var manager = new AddGroupManager { SetWaiting = isBusy => SetWaiting(isBusy) };
            var dialog = new AddGroupScreen(manager);
            dialog.Closed += async (sender, args) =>
            {
                Refresh();
            };
            dialog.Show();
        }
        private void Edit()
        {
            if (SelectedGroup == null)
                return;
            SetWaiting(true);
            var addPaymentManager = new AddGroupManager(SelectedGroup);
            addPaymentManager.SetWaiting = isBusy => SetWaiting(isBusy);
            var addPaymentDialog = new AddGroupScreen(addPaymentManager);
            addPaymentDialog.Closed += async (sender, args) =>
            {
                Refresh();
            };
            addPaymentDialog.Show();
        }
        private async void Delete()
        {
            if (SelectedGroup == null)
                return;
            bool res = await DialogHelper.ShowAffirmationDialog("Подтвердите удаление объекта", "Вы уверен что хотите удалить данный объект? Данное действие необратимо");
            if (!res)
                return;
            var service = Get<IGeneralService>();
            service.DeleteGroup(SelectedGroup);
            Refresh();
        }
        public override void Refresh()
        {
            SetWaiting(true);
            var service = Get<IGeneralService>();
            Groups = service.GetAllGroups();
            RaisePropertyChanged("Groups");
            SetWaiting(false);
        }
    }
}
