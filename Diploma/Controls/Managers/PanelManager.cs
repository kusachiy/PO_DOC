using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Controls.Managers
{
    class PanelManager:ViewModelBase
    {     
        public List<PanelButtonManager> LeftButtons { get; set; }

        public List<PanelButtonManager> MiddleButtons { get; set; }

        public List<PanelButtonManager> RightButtons { get; set; }
    }
}
