using CommonServiceLocator;
using Diploma.Controls.Managers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Diploma.Screens.Managers
{
    public abstract class BaseManager:ViewModelBase
    {
        private ConcurrentDictionary<string, List<string>> _errors = new ConcurrentDictionary<string, List<string>>();
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        private object _lock = new object();

        public RelayCommand Loaded { get; set; }
        private bool _waiting ;
        public Action<bool> SetWaiting { get; set; }
        public bool Waiting {
            get
            {
                return _waiting;
            }
            set
            {
                if (value == _waiting)
                    return;
                _waiting = value;
                RaisePropertyChanged();
            }
        }

        public ViewModelBase Panel { get; set; } 

        public abstract void Refresh();

        protected BaseManager()
        {
            Loaded = new RelayCommand(Refresh);
        }

        protected TService Get<TService>()
        {
            return ServiceLocator.Current.GetInstance<TService>();
        }


        public IEnumerable GetErrors(string propertyName)
        {
            List<string> errorsForName;
            _errors.TryGetValue(propertyName, out errorsForName);
            return errorsForName;
        }

        public bool HasErrors
        {
            get { return _errors.Any(kv => kv.Value != null && kv.Value.Count > 0); }
        }

        public Task ValidateAsync()
        {
            return Task.Run(() => Validate());
        }

        public void Validate()
        {
            lock (_lock)
            {
                var validationContext = new ValidationContext(this, null, null);
                var validationResults = new List<ValidationResult>();
                Validator.TryValidateObject(this, validationContext, validationResults, true);

                foreach (var kv in _errors.ToList())
                {
                    if (validationResults.All(r => r.MemberNames.All(m => m != kv.Key)))
                    {
                        List<string> outLi;
                        _errors.TryRemove(kv.Key, out outLi);
                        OnErrorsChanged(kv.Key);
                    }
                }

                var q = from r in validationResults
                        from m in r.MemberNames
                        group r by m into g
                        select g;

                foreach (var prop in q)
                {
                    var messages = prop.Select(r => r.ErrorMessage).ToList();

                    if (_errors.ContainsKey(prop.Key))
                    {
                        List<string> outLi;
                        _errors.TryRemove(prop.Key, out outLi);
                    }
                    _errors.TryAdd(prop.Key, messages);
                    OnErrorsChanged(prop.Key);
                }
            }
        }

        public void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }
}
