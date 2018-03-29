using Diploma.Controls.Managers;
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
    class FacultiesManager:BaseManager
    {
        public ObservableCollection<Faculty> Faculties { get; set; }
        private Faculty _selectedFaculty;
        public Faculty SelectedFaculty { get { return _selectedFaculty; } set { _selectedFaculty = value; RaisePropertyChanged(); } }

        public FacultiesManager()
        {
            Panel = new PanelManager
            {
                LeftButtons = new List<PanelButtonManager>
                {
                    new PanelButtonManager{OnButtonAction = o=> Add(),Icon = PackIconModernKind.Add,Text = "Add"},
                    new PanelButtonManager{OnButtonAction =o=>Delete(),Icon = PackIconModernKind.Delete,Text="Delete"}
                },

                RightButtons = new List<PanelButtonManager>
                {
                    new PanelButtonManager{OnButtonAction = o=> Save(),Icon = PackIconModernKind.Save,Text = "Save"},
                }
            };
        }

        private async void Delete()
        {
            if (SelectedFaculty == null)
                return;
            bool res = await DialogHelper.ShowAffirmationDialog("Подтвердите удаление объекта", "Вы уверен что хотите удалить данный объект? Данное действие необратимо");
            if (!res)
                return;
            var service = Get<IGeneralService>();
            service.DeleteFaculty(SelectedFaculty);
            Refresh();
        }

        private void Add()
        {
            var d = new Faculty();
            Faculties.Add(d);
            SelectedFaculty = d;
            RaisePropertyChanged("Faculties");
        }

        private async void Save()
        {
            var service = Get<IGeneralService>();
            foreach (var dep in Faculties)
                service.AddOrUpdateFaculty(dep);
            await DialogHelper.ShowMessageDialog("Сохранено", "");
        }        
        public override void Refresh()
        {
            SetWaiting(true);
            var service = Get<IGeneralService>();
            try
            {
                Faculties = new ObservableCollection<Faculty>(service.GetAllFaculties());
            }
            catch (Exception e)
            {              
                return;
            }
            RaisePropertyChanged("Faculties");
            SetWaiting(false);
        }
    }
}
