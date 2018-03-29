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
    class EmployeesManager:BaseManager
    {
        public ObservableCollection<Employee> Employees { get; set; }
        public string[] Degrees { get; set; }
        public string[] Ranks { get; set; }
        public string[] Positions { get; set; }

        private Employee _selectedEmployee;
        public Employee SelectedEmployee { get { return _selectedEmployee; } set { _selectedEmployee = value; RaisePropertyChanged(); } }

        public EmployeesManager()
        {
            Panel = new PanelManager
            {
                LeftButtons = new List<PanelButtonManager>
                {
                    new PanelButtonManager{OnButtonAction = o=> Add(),Icon = PackIconModernKind.Add,Text = "Add"},
                    new PanelButtonManager{OnButtonAction = o=> Edit(),Icon = PackIconModernKind.Edit,Text = "Edit"},
                    new PanelButtonManager{OnButtonAction =o=>Delete(),Icon = PackIconModernKind.Delete,Text="Delete"}

                }
            };
            Degrees = EnumExtender.GetAllDescriptions(typeof(AcademicDegree));
            Ranks = EnumExtender.GetAllDescriptions(typeof(AcademicRank));
            Positions = EnumExtender.GetAllDescriptions(typeof(WorkingPosition));
        }

        private void Edit()
        {
            if (SelectedEmployee == null)
                return;
            SetWaiting(true);
            var addPaymentManager = new AddEmployeeManager { SetWaiting = isBusy => SetWaiting(isBusy) };
            addPaymentManager = new AddEmployeeManager(SelectedEmployee);
            var addPaymentDialog = new AddEmployeeScreen(addPaymentManager);
            addPaymentDialog.Closed += async (sender, args) =>
            {
                Refresh();
            };
            addPaymentDialog.Show();
        }

        private async void Delete()
        {
            if (SelectedEmployee == null)
                return;
            bool res = await DialogHelper.ShowAffirmationDialog("Подтвердите удаление объекта", "Вы уверен что хотите удалить данный объект? Данное действие необратимо");
            if (!res)
                return;
            var service = Get<IGeneralService>();
            service.DeleteEmployee(SelectedEmployee);
            Refresh();
        }

        private void Add()
        {            
            SetWaiting(true);
            var manager = new AddEmployeeManager { SetWaiting = isBusy => SetWaiting(isBusy) };
            var dialog = new AddEmployeeScreen(manager);
            dialog.Closed += async (sender, args) =>
            {
                Refresh();             
            };
            dialog.Show();
        }

        private async void Save()
        {
            var service = Get<IGeneralService>();
            foreach (var emp in Employees)
                service.AddOrUpdateEmployee(emp);
            await DialogHelper.ShowMessageDialog("Сохранено", "");
        }        
        public override void Refresh()
        {
            SetWaiting(true);
            var service = Get<IGeneralService>();            
            Employees = new ObservableCollection<Employee>(service.GetAllEmployees());
            RaisePropertyChanged("Degrees");
            RaisePropertyChanged("Ranks");
            RaisePropertyChanged("Positions");
            RaisePropertyChanged("Employees");
            SetWaiting(false);
        }
    }
}
