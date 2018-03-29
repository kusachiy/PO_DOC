using Diploma.Screens.Managers;
using GalaSoft.MvvmLight;
using MahApps.Metro.IconPacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma
{
    class TabScreenManager:ViewModelBase
    {
        public BaseManager Screen { get; set; }
        public PackIconModernKind Icon { get; set; }
        public string Label { get; set; }
    }
}
