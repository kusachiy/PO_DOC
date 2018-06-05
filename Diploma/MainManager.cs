using DBRepository;
using DBRepository.Migrations;

using Diploma.Dialogs;
using Diploma.Dialogs.Managers;
using Diploma.Screens.Managers;
using Diploma.Services;
using Diploma.Utils;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Diploma
{
    class MainManager : BaseManager
    {
        public string Title => "Система документооборота кафедры ПО";
        public ObservableCollection<TabScreenManager> TabScreens { get; set; }
        private TabScreenManager _selectedTab;
        public TabScreenManager SelectedTab
        {
            get
            {
                return _selectedTab;
            }
            set
            {
                if (value == _selectedTab)
                    return;
                _selectedTab = value;
                RaisePropertyChanged();
            }
        }
        private DepartmentsManager _departmentsManager;
        private DisciplinesManager _disciplinesManager;
        private EmployeesManager _employeesManager;
        private FacultiesManager _facultiesManager;
        private GroupsManager _groupsManager;
        private SpecialitiesManager _specialitiesManager;
        private DisciplineWorkloadManager _disciplineWorkloadManager;
        private WorkloadManager _workloadManager;


        public RelayCommand ShowSemestersDialog { get; set; }
        public RelayCommand ImportCommand { get; set; }
        public RelayCommand ExportCommand { get; set; }
        public RelayCommand ImportSettingsCommand { get; set; }
        public RelayCommand ExportSettingsCommand { get; set; }
        public RelayCommand ShowSpecialPositionsDialog { get; set; }



        public MainManager()
        {            
             //Database.SetInitializer(new MigrateDatabaseToLatestVersion<Context, Configuration>());
            _departmentsManager = new DepartmentsManager { SetWaiting = status => Waiting = status };
            _disciplinesManager = new DisciplinesManager{ SetWaiting = status => Waiting = status };
            _employeesManager = new EmployeesManager { SetWaiting = status => Waiting = status };
            _facultiesManager = new FacultiesManager{ SetWaiting = status => Waiting = status };
            _groupsManager = new GroupsManager { SetWaiting = status => Waiting = status };
            _specialitiesManager = new SpecialitiesManager { SetWaiting = status => Waiting = status };
            _disciplineWorkloadManager = new DisciplineWorkloadManager { SetWaiting = status => Waiting = status };
            _workloadManager = new WorkloadManager { SetWaiting = status => Waiting = status };

            ShowSemestersDialog = new RelayCommand(OpenSettings);
            ImportCommand = new RelayCommand(Import);
            ExportCommand = new RelayCommand(Export);
            ImportSettingsCommand = new RelayCommand(ImportSettings);
            ExportSettingsCommand = new RelayCommand(ExportSettings);
            ShowSpecialPositionsDialog = new RelayCommand(ShowSpecialPositions);
            RefreshTabs();
        }

        private void ShowSpecialPositions()
        {
            var manager = new SpecialPositionsManager { SetWaiting = status => Waiting = status };
            var dialog = new SpecialPositionsDialog(manager);
            dialog.Show();
        }

        private void Export()
        {
            var manager = new ExportManager { SetWaiting = status => Waiting = status };
            var dialog = new ExportDialog(manager);
            dialog.Show();
        }

        private void Import()
        {
            var manager = new ImportManager { SetWaiting = status => Waiting = status };
            var dialog = new ImportDialog(manager);
            dialog.Show();
        }

        private void ExportSettings()
        {
            
        }

        private void ImportSettings()
        {
            var manager = new ImportSettingsManager();
            var dialog = new ImportSettingsDialog(manager);
            dialog.Show();
        }

        private void OpenSettings()
        {
            var manager = new SemesterSettingsManager();
            var dialog = new SemesterSettingsDialog(manager);            
            dialog.Show();
        }

        private void RefreshTabs()
        {
            TabScreens = new ObservableCollection<TabScreenManager>
            {
                new TabScreenManager
                {
                    Icon = MahApps.Metro.IconPacks.PackIconModernKind.GradeD,
                    Label = "Кафедры",
                    Screen = _departmentsManager
                },
                new TabScreenManager
                {
                    Icon = MahApps.Metro.IconPacks.PackIconModernKind.Puzzle,
                    Label = "Дисциплины",
                    Screen = _disciplinesManager
                },
                new TabScreenManager
                {
                    Icon = MahApps.Metro.IconPacks.PackIconModernKind.FuturamaFry,
                    Label = "Преподаватели",
                    Screen = _employeesManager
                },
                new TabScreenManager
                {
                    Icon = MahApps.Metro.IconPacks.PackIconModernKind.GradeF,
                    Label = "Факультеты",
                    Screen = _facultiesManager
                },
                new TabScreenManager
                {
                    Icon = MahApps.Metro.IconPacks.PackIconModernKind.Group,
                    Label = "Группы",
                    Screen = _groupsManager
                },
                new TabScreenManager
                {
                    Icon = MahApps.Metro.IconPacks.PackIconModernKind.Tag,
                    Label = "Специальности",
                    Screen = _specialitiesManager
                },
                 new TabScreenManager
                {
                    Icon = MahApps.Metro.IconPacks.PackIconModernKind.BookPerspective,
                    Label = "Нагрузки за семестр",
                    Screen = _disciplineWorkloadManager
                },
                new TabScreenManager
                {
                    Icon = MahApps.Metro.IconPacks.PackIconModernKind.Pin,
                    Label = "Назначение преподавателей",
                    Screen = _workloadManager
                }
            };
            SelectedTab = TabScreens.FirstOrDefault();
            RaisePropertyChanged("TabScreens");
        }

        public override void Refresh()
        {
            
        }
    }   
}
