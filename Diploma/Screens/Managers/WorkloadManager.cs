using Diploma.Controls.Managers;
using Diploma.Services;
using MahApps.Metro.IconPacks;
using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Screens.Managers
{
    class WorkloadManager:BaseManager
    {        
        public List<Workload> Workloads{ get; set; }
        public WorkloadManager()
        {
            Panel = new PanelManager
            {
                LeftButtons = new List<PanelButtonManager>
                {
                    new PanelButtonManager{OnButtonAction = o=> Add(),Icon = PackIconModernKind.Add,Text = "Add"}
                }               
            };
        }       

        private void Add()
        {

        }

        
        
        public override void Refresh()
        {
            SetWaiting(true);
            var service = Get<IGeneralService>();
            Workloads = service.GetAllWorkloads();
            RaisePropertyChanged("Workloads");
            SetWaiting(false);
        }
    }
}
