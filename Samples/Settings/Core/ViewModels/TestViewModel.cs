using System;
using System.Windows.Input;

using Cheesebaron.MvxPlugins.Settings.Interfaces;

using MvvmCross.Core.ViewModels;

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
        private string _boolKey = "cheesey.bool";

        public string StringKey
        {
            get { return _stringKey; }
            set
            {
                _stringKey = value;
                RaisePropertyChanged(() => StringKey);
            }
        }

        public string BoolKey
        {
            get { return _boolKey;}
            set
            {
                _boolKey = value;
                RaisePropertyChanged(() => BoolKey);
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

        private bool _boolSetting;
        public bool BoolSetting
        {
            get { return _boolSetting; }
            set
            {
                _boolSetting = value;
                RaisePropertyChanged(() => BoolSetting);
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

        private bool _restoredBoolSetting;
        public bool RestoredBoolSetting
        {
            get { return _restoredBoolSetting; }
            set
            {
                _restoredBoolSetting = value;
                RaisePropertyChanged(() => RestoredBoolSetting);
            }
        }
        
        private MvxCommand _saveSettingsCommand;
        public ICommand SaveSettingsCommand
        {
            get
            {
                _saveSettingsCommand = _saveSettingsCommand ?? new MvxCommand(DoSaveSettingsCommand, () => true);
                return _saveSettingsCommand;
            }
        }

        private void DoSaveSettingsCommand()
        {
            _settings.AddOrUpdateValue(StringKey, StringSetting);
            _settings.AddOrUpdateValue(BoolKey, BoolSetting);

            _settings.AddOrUpdateValue("Guid", Guid.NewGuid());
        }

        private MvxCommand _restoreSettingsCommand;
        public ICommand RestoreSettingsCommand
        {
            get
            {
                _restoreSettingsCommand = _restoreSettingsCommand ?? new MvxCommand(DoRestoreSettingsCommand, () => true);
                return _restoreSettingsCommand;
            }
        }

        private Guid _guid;
        private void DoRestoreSettingsCommand()
        {
            RestoredStringSetting = _settings.GetValue(StringKey,"<Empty>");
            RestoredBoolSetting = _settings.GetValue(BoolKey, false);

            _guid = _settings.GetValue("Guid", Guid.Empty);
        }
    }
}
