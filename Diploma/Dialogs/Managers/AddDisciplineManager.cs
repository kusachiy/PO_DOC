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

    public class AddDisciplineManager : BaseManager
    {
        public Action OnExit { get; set; }
        public string Title { get; set; }
        private Discipline _discipline;
        public Discipline Discipline { get { return _discipline; } set { _discipline = value;RaisePropertyChanged(); } }
        public string[] Types { get; set; }

          
        public ObservableCollection<Department> Departments { get; private set; }

        public AddDisciplineManager()
        {
            Discipline = new Discipline();
            Title = "Добавить дисциплину";
            Initialize();
        }
        public AddDisciplineManager(Discipline discipline)
        {
            Discipline = discipline;
            Title = "Изменить дисциплину";
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
            Types = EnumExtender.GetAllDescriptions(typeof(SpecialDisciplineKind));
        }

        private void Save()
        {          
            var service = Get<IGeneralService>();
            service.AddOrUpdateDiscipline(_discipline);
            OnExit();
        }

        public override async void Refresh()
        {
            var service = Get<IGeneralService>();
            Departments = new ObservableCollection<Department>(service.GetAllDepartments());
            RaisePropertyChanged("Departments");
        }
    }
}
