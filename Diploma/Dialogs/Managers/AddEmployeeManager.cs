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

    public class AddEmployeeManager : BaseManager
    {
        public Action OnExit { get; set; }
        public string Title { get; set; }
        private Employee _employee;
        public Employee Employee { get { return _employee; } set { _employee = value;RaisePropertyChanged(); } }

        public string[] Degrees { get; private set; }
        public string[] Ranks { get; private set; }
        public string[] Positions { get; private set; }

        public AddEmployeeManager()
        {
            Employee = new Employee();
            Title = "Добавить сотрудника";
            Initialize();
        }
        public AddEmployeeManager(Employee employee)
        {
            Employee = employee;
            Title = "Изменить сотрудника";
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
            Degrees = EnumExtender.GetAllDescriptions(typeof(AcademicDegree));
            Ranks = EnumExtender.GetAllDescriptions(typeof(AcademicRank));
            Positions = EnumExtender.GetAllDescriptions(typeof(WorkingPosition));
        }

        private void Save()
        {
            var service = Get<IGeneralService>();
            service.AddOrUpdateEmployee(_employee);
            OnExit();
        }

        public override async void Refresh()
        {
           
        }
    }
}
