using Diploma.Controls.Managers;
using Diploma.Screens.Managers;
using Diploma.Services;
using Diploma.Utils;
using MahApps.Metro.IconPacks;
using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Diploma.Dialogs.Managers
{   

    public class ImportSettingsManager : BaseManager
    {
        public Action OnExit { get; set; }

        public ImportSettingsManager()
        {
            Panel = new PanelManager
            {
                MiddleButtons = new List<PanelButtonManager>
                {
                    new PanelButtonManager{OnButtonAction=o=>Refresh(),Icon = PackIconModernKind.Refresh,Text="Refresh"}
                },
                RightButtons = new List<PanelButtonManager>
                {
                    new PanelButtonManager{OnButtonAction = o=> Save(),Icon = PackIconModernKind.Save,Text = "Save"}                  
                }
            };
        }

        private void Save()
        {           
            Properties.Import.Default.Save();
            OnExit();
        }

        public override async void Refresh()
        {
            Properties.Import.Default.Reload();
        }
    }
}
