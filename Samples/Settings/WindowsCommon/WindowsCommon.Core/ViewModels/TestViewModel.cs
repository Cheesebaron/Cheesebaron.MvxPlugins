using System;
using System.Windows.Input;
using Cheesebaron.MvxPlugins.Settings.Interfaces;
using Cirrious.MvvmCross.ViewModels;

namespace WindowsCommon.Core.ViewModels
{
    public class TestViewModel 
        : MvxViewModel
    {
        private readonly ISettings _settings;

        public TestViewModel(ISettings settings) {
            _settings = settings;
        }

        private DateTimeOffset _selectedDate;

        public DateTimeOffset SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                _selectedDate = value; 
                RaisePropertyChanged(() => SelectedDate);
            }
        }

        private MvxCommand _saveCommand;

        public ICommand SaveCommand
        {
            get { return _saveCommand = _saveCommand ?? new MvxCommand(DoSaveCommand); }
        }

        private MvxCommand _restoreCommand;

        public ICommand RestoreCommand
        {
            get { return _restoreCommand = _restoreCommand ?? new MvxCommand(DoRestoreCommand); }
        }

        private void DoRestoreCommand()
        {
            SelectedDate = _settings.GetValue("date", DateTimeOffset.Now);
        }

        private void DoSaveCommand() { _settings.AddOrUpdateValue("date", SelectedDate); }
    }
}
