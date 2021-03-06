﻿using Diploma.Controls.Managers;
using Diploma.Screens.Managers;
using Diploma.Utils.ExcelHelpers;
using GalaSoft.MvvmLight.Command;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.IconPacks;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace Diploma.Dialogs.Managers
{

    public class ImportManager : BaseManager
    {
        public Action OnExit { get; set; }
        public string[] Paths { get; set; }
        public string Files => Paths==null? "": $"Добавлено {Paths.Length} файлов.";
        public string Log { get; set; }
        public bool NewInputFormat { get; set; } = true;

        public int Year { get; set; }
        public RelayCommand SelectFileCommand { get; set; }
        private IImorter _excelmporter;
        private PanelButtonManager _panelButtonManager;
        private PanelButtonManager _secondManager;       

        public ImportManager()
        {
            Year = DateTime.Now.Year;
            _panelButtonManager = new PanelButtonManager { OnButtonAction = o => Import(), Icon = PackIconModernKind.OfficeExcel, Text = "Импорт" };
            _secondManager = new PanelButtonManager {OnButtonAction = o => Save(), Icon = PackIconModernKind.NavigateNext, Text = "Продолжить",ButtonVisibility=Visibility.Collapsed };
            Panel = new PanelManager
            {
                RightButtons =  new List<PanelButtonManager>
                {
                    _panelButtonManager,_secondManager,new PanelButtonManager{OnButtonAction= o=>OnExit(),Icon=PackIconModernKind.Cancel,Text = "Закрыть"}
                }
            };
            SelectFileCommand = new RelayCommand(SelectFile);
        }

        private async void Import()
        {
            if (NewInputFormat)
                await NewImport();
            else
                await OldImport();
        }

        private async Task NewImport()
        {
            if (Paths == default(string[]) || Paths.Length == 0)
                return;
            Waiting = true;
            _excelmporter = new Excelmporter();
            bool isSuccess = true;
            foreach (var file in Paths)
            {
                isSuccess = await Task.Run(() => _excelmporter.ImportDataFromExcel(file, Year)) && isSuccess;
            }
            Log = "";
            foreach (var str in _excelmporter.Log)
            {
                Log += $"{str}\r\n";
            }
            if (_excelmporter.Log.Count != 0)
            {
                if (isSuccess)
                    MessageBox.Show("В данных обнаружены неизвестные элементы. Замечания полученные в ходе импорта данных приведены в логе. В случае продолжения работы недостающие данные будут автоматически созданы и добалены. Убедитесь в правильности исходных данных...");
                else
                {
                    MessageBox.Show("Фатальная ошибка");
                    Waiting = false;
                    RaisePropertyChanged("Log");
                    return;
                }
            }
            else
                MessageBox.Show("Данные успешно считаны. Нажмите 'Продолжить', чтобы занести их в базу");
            _secondManager.ButtonVisibility = Visibility.Visible;
            _panelButtonManager.ButtonVisibility = Visibility.Collapsed;
            _panelButtonManager.RaisePropertyChanged("Panel");
            RaisePropertyChanged("Log");
            Waiting = false;
        }

        private async Task OldImport()
        {
            if (Paths == default(string[]) || Paths.Length == 0)
                return;
            Waiting = true;
            _excelmporter = new OldExcelImporter();
            bool isSuccess = true;
            var file = Paths[0];           
            isSuccess = await Task.Run(() => _excelmporter.ImportDataFromExcel(file, Year));           
            Log = "";
            foreach (var str in _excelmporter.Log)
            {
                Log += $"{str}\r\n";
            }
            if (_excelmporter.Log.Count != 0)
            {
                if (isSuccess)
                    MessageBox.Show("В данных обнаружены неизвестные элементы. Замечания полученные в ходе импорта данных приведены в логе. В случае продолжения работы недостающие данные будут автоматически созданы и добалены. Убедитесь в правильности исходных данных...");
                else
                {
                    MessageBox.Show("Фатальная ошибка");
                    Waiting = false;
                    RaisePropertyChanged("Log");
                    return;
                }
            }
            else
                MessageBox.Show("Данные успешно считаны. Нажмите 'Продолжить', чтобы занести их в базу");
            _secondManager.ButtonVisibility = Visibility.Visible;
            _panelButtonManager.ButtonVisibility = Visibility.Collapsed;
            _panelButtonManager.RaisePropertyChanged("Panel");
            RaisePropertyChanged("Log");
            Waiting = false;
        }

        private void SelectFile()
        {
            OpenFileDialog myDialog = new OpenFileDialog
            {
                Filter = "Файлы с набором дисциплин Excel(*.xls*)|*.xls*;",
                CheckFileExists = true,
                Multiselect = true
            };
            if (myDialog.ShowDialog() == true)
            {
                Paths = myDialog.FileNames;
            }
            RaisePropertyChanged("Files");
        }

        private async void Save()
        {
            Waiting = true;
            Log = "";
            await Task.Run(() => _excelmporter.SaveData());
            foreach (var str in _excelmporter.Log)
            {
                Log += $"{str}\r\n";
            }
            _secondManager.ButtonVisibility = Visibility.Collapsed;
            MessageBox.Show("Операция успешно завершена. Все изменения внесены в базу данных");
            RaisePropertyChanged("Log");
            Waiting = false;
        }

        public override async void Refresh()
        {

        }
    }
}
