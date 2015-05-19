using System.Windows.Input;

using Cheesebaron.MvxPlugins.Settings.Interfaces;

using Cirrious.MvvmCross.ViewModels;

namespace Core.ViewModels
{
    public class TestViewModel
        : MvxViewModel
    {
        private readonly ISettings _settings;

        public TestViewModel(ISettings settings)
        {
            _settings = settings;
            RestoredStringSetting = "<Empty>";
        }

        private string _stringKey = "cheesey.string";

        public string StringKey
        {
            get { return _stringKey; }
            set
            {
                _stringKey = value;
                RaisePropertyChanged(() => StringKey);
            }
        }

        private string _stringSetting;
        public string StringSetting
        {
            get { return _stringSetting; }
            set
            {
                _stringSetting = value;
                RaisePropertyChanged(() => StringSetting);
            }
        }

        private string _restoredStringSetting;
        public string RestoredStringSetting
        {
            get { return _restoredStringSetting; }
            set
            {
                _restoredStringSetting = value;
                RaisePropertyChanged(() => RestoredStringSetting);
            }
        }
        
        private MvxCommand _saveSettingsCommand;
        public ICommand SaveSettingsCommand
        {
            get
            {
                _saveSettingsCommand = _saveSettingsCommand ?? new MvxCommand(DoSaveSettingsCommand);
                return _saveSettingsCommand;
            }
        }

        private void DoSaveSettingsCommand()
        {
            _settings.AddOrUpdateValue(StringKey, _stringSetting);
        }

        private MvxCommand _restoreSettingsCommand;
        public ICommand RestoreSettingsCommand
        {
            get
            {
                _restoreSettingsCommand = _restoreSettingsCommand ?? new MvxCommand(DoRestoreSettingsCommand);
                return _restoreSettingsCommand;
            }
        }

        private void DoRestoreSettingsCommand()
        {
            RestoredStringSetting = _settings.GetValue(StringKey, "<Empty>");
        }
    }
}
