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

    public class AddSpecialityManager : BaseManager
    {
        public Action OnExit { get; set; }
        public string Title { get; set; }
        private Speciality _speciality;
        public Speciality Speciality { get { return _speciality; } set { _speciality = value;RaisePropertyChanged(); } }
    
        public ObservableCollection<Faculty> Faculties { get; private set; }

        public AddSpecialityManager()
        {
            Speciality = new Speciality();
            Title = "Добавить группу";
            Initialize();
        }
        public AddSpecialityManager(Speciality speciality)
        {
            Speciality = speciality;
            Title = "Изменить группу";
            Initialize();
        }
        private void Initialize()
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
            service.AddOrUpdateSpeciality(_speciality);
            OnExit();
        }

        public override async void Refresh()
        {
            var service = Get<IGeneralService>();
            Faculties = new ObservableCollection<Faculty>(service.GetAllFaculties());
            RaisePropertyChanged("Faculties");
        }
    }
}
