using Diploma.Dialogs;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Diploma.Utils
{
    public static class DialogHelper
    {
        public static async Task<string> ShowInputDialog(string title, string message)
        {
            var metroWindow = Application.Current.MainWindow as MetroWindow;
            metroWindow.MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Accented;            
            return await metroWindow.ShowInputAsync(title, message);
        }        
        public static async Task ShowMessageDialog(string title, string message)
        {
            var metroWindow = Application.Current.MainWindow as MetroWindow;
            metroWindow.MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Accented;
            await metroWindow.ShowMessageAsync(title, message);
        }
        public static async Task<bool> ShowAffirmationDialog(string title, string message)
        {
            var deleteDialogSettings = new MetroDialogSettings
            {
                AffirmativeButtonText = "Да",
                NegativeButtonText = "Нет",
                FirstAuxiliaryButtonText = "Отмена"
            };

            var metroWindow = Application.Current.MainWindow as MetroWindow;
            if (metroWindow == null)
                return false;
            var result =
                await
                    metroWindow.ShowMessageAsync(title, message,
                        MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary, deleteDialogSettings);
            return (result == MessageDialogResult.Affirmative);

        }

        public static void ActivateDialog(Type window)
        {
            MetroWindow dialog = null;
            for (int i = 0; i < Application.Current.Windows.Count; i++)
            {
                var currentWindow = Application.Current.Windows[i];
                if (currentWindow != null && currentWindow.GetType() == window)
                {
                    dialog = Application.Current.Windows[i] as ImportDialog;
                    break;
                }
            }
            dialog?.Activate();
        }
    }
}
