using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MahApps.Metro.IconPacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Diploma.Controls.Managers
{
    class PanelButtonManager:ViewModelBase
    {
        private Action<object> _onButtonAction;

        public Action<object> OnButtonAction
        {
            get { return _onButtonAction; }
            set
            {
                _onButtonAction = value;
                ButtonAction = new RelayCommand<object>(OnButtonAction);
                RaisePropertyChanged("OnButtonAction");
            }
        }

        public RelayCommand<object> ButtonAction { get; set; }

        private PackIconModernKind icon;

        public PackIconModernKind Icon
        {
            get { return icon; }
            set
            {
                if (icon == value)
                    return;
                icon = value;
                RaisePropertyChanged("Icon");
            }
        }
        private string _text;

        public string Text
        {
            get { return _text; }
            set
            {
                if (_text == value)
                    return;
                _text = value;
                RaisePropertyChanged("Text");
            }
        }
        private Visibility _buttonVisibility = Visibility.Visible;
        public Visibility ButtonVisibility
        {
            get { return _buttonVisibility; }
            set
            {
                if (_buttonVisibility == value)
                    return;
                _buttonVisibility = value;
                RaisePropertyChanged("ButtonVisibility");
            }
        }
    }
}
