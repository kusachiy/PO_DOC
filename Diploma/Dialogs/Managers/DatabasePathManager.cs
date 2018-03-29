using Diploma.Controls.Managers;
using Diploma.Screens.Managers;
using GalaSoft.MvvmLight.Command;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.IconPacks;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace Diploma.Dialogs.Managers
{

    public class DatabasePathManager : BaseManager
    {
        public Action OnExit { get; set; }
        public string Path { get; set; }
        public RelayCommand SelectFileCommand { get; set; }
       
        public DatabasePathManager()
        {
            Panel = new PanelManager
            {                
                RightButtons = new List<PanelButtonManager>
                {
                    new PanelButtonManager{OnButtonAction = o=> Save(),Icon = PackIconModernKind.Save,Text = "Save"}
                }
            };    
            SelectFileCommand = new RelayCommand(SelectFile);
        }

        private void SelectFile()
        {
            OpenFileDialog myDialog = new OpenFileDialog();
            myDialog.Filter = "Базы данных Access(*.accdb)|*.ACCDB;";
            myDialog.CheckFileExists = true;
            myDialog.Multiselect = true;
            if (myDialog.ShowDialog() == true)
            {
                Path = myDialog.FileName;
            }
            RaisePropertyChanged("Path");
        }

        private void Save()
        {
            if (Path == default(string))
                return;
            try
            {                
                if(!Directory.Exists(System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"Database")))
                    Directory.CreateDirectory(System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"Database"));
                File.Copy(Path, System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"Database\database.accdb"));
                MessageBox.Show("База данных успешно установлена");
                OnExit();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удается скопировать файл из-за исключения: " + ex.Message);
            }
        }

        public override async void Refresh()
        {
            
        }
    }
}
