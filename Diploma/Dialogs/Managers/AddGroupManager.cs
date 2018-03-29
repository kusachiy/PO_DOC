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

    public class AddGroupManager : BaseManager
    {
        public Action OnExit { get; set; }
        public string Title { get; set; }
        private Group _group;
        public Group Group { get { return _group; } set { _group = value;RaisePropertyChanged(); } }

        public string[] Qualifications { get; private set; }
        public string[] StudyForms { get; private set; }
        public ObservableCollection<Speciality> Specialities { get; private set; }

        public AddGroupManager()
        {
            Group = new Group();
            Title = "Добавить группу";
            Initialize();
        }
        public AddGroupManager(Group group)
        {
            Group = group;
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
            Qualifications = EnumExtender.GetAllDescriptions(typeof(Qualification));
            StudyForms = EnumExtender.GetAllDescriptions(typeof(StudyForm));          
        }

        private void Save()
        {
            var service = Get<IGeneralService>();
            service.AddOrUpdateGroup(_group);
            OnExit();
        }

        public override async void Refresh()
        {
            var service = Get<IGeneralService>();
            Specialities = new ObservableCollection<Speciality>(service.GetAllSpecialities());
            RaisePropertyChanged("Specialities");
        }
    }
}
