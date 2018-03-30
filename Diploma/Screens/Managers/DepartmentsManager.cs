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
using System.Windows;

namespace Diploma.Screens.Managers
{
    class DepartmentsManager:BaseManager
    {        
        public ObservableCollection<Department> Departments{ get; set; }
        private Department _selectedDepartment;
        public Department SelectedDepartment { get { return _selectedDepartment; } set { _selectedDepartment = value;RaisePropertyChanged(); } }

        public DepartmentsManager()
        {
            Panel = new PanelManager
            {
                LeftButtons = new List<PanelButtonManager>
                {
                    new PanelButtonManager{OnButtonAction = o=> Add(),Icon = PackIconModernKind.Add,Text = "Add"},
                    new PanelButtonManager{OnButtonAction =o=>Delete(),Icon = PackIconModernKind.Delete,Text="DeleteRow"}

                },

                RightButtons = new List<PanelButtonManager>
                {
                    new PanelButtonManager{OnButtonAction = o=> Save(),Icon = PackIconModernKind.Save,Text = "Save"},
                }
            };
        }

        private async void Delete()
        {
            if (SelectedDepartment == null)
                return;
            bool res = await DialogHelper.ShowAffirmationDialog("Подтвердите удаление объекта", "Вы уверен что хотите удалить данный объект? Данное действие необратимо");
            if (!res)
                return;
            var service = Get<IGeneralService>();
            service.DeleteDepartment(SelectedDepartment);
            Refresh();
        }

        private void Add()
        {
            var d = new Department();
            Departments.Add(d);
            SelectedDepartment = d;
            RaisePropertyChanged("Departments");
        }

        private async void Save()
        {
            var service = Get<IGeneralService>();
            foreach (var dep in Departments)
                service.AddOrUpdateDepartment(dep);
            await DialogHelper.ShowMessageDialog("Сохранено", "");
        }
        private void OpenDBPathDialog()
        {
            var manager = new DatabasePathManager();
            var dialog = new DatabasePathDialog(manager);
            if(dialog.ShowDialog()==true)
            {
                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);//restart
            }
            Application.Current.Shutdown();
        }
        public override void Refresh()
        {
            SetWaiting(true);
            var service = Get<IGeneralService>();
            try
            {
                Departments = new ObservableCollection<Department>(service.GetAllDepartments());
            }
            catch (Exception e)
            {
                OpenDBPathDialog();
                return;
            }
            RaisePropertyChanged("Departments");
            SetWaiting(false);
        }
    }
}
