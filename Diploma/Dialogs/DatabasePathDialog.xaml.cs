using Diploma.Dialogs.Managers;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Diploma.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class DatabasePathDialog:MetroWindow
    {
        public DatabasePathDialog(DatabasePathManager databasePathManager)
        {
            InitializeComponent();
            DataContext = databasePathManager;
            databasePathManager.OnExit = () => { DialogResult = true; Close(); };

        }

        private void MetroWindow_LostFocus(object sender, RoutedEventArgs e)
        {
          
        }
    }
}
