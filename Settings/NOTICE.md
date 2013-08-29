Derivative work is based on https://github.com/ceton/Mvx.Plugins.Settings/tree/daf416b4b8d91baad98d6c5e29565edaf3123a74

Where the following file mapping is present:

ceton.mvx.plugins.settings/ISettings.cs -> Cheesebaron.MvxPlugins.Settings/Interfaces/ISettings.cs
ceton.mvx.plugins.settings.Droid/MvxAndroidSettings.cs -> Cheesebaron.MvxPlugins.Settings.Droid/Settings.cs
ceton.mvx.plugins.settings.Touch/MvxTouchSettings.cs -> Cheesebaron.MvxPlugins.Settings.Touch/Settings.cs
ceton.mvx.plugins.settings.WindowsPhone/MvxWindowsPhoneSettings.cs -> Cheesebaron.MvxPlugins.Settings.WindowsPhone/Settings.cs


Cheesebaron.MvxPlugins.Settings/ISettings.cs
============================================

- Modified `AddOrUpdateValue` to be generic`
- Added interfaces `DeleteValue`, `Contains` and `ClearAllValues`
- Removed interface `Save`

Cheesebaron.MvxPlugins.Settings.Droid/Settings.cs
Cheesebaron.MvxPlugins.Settings.Touch/Settings.cs
========================================================

- Added throwing of exception when unknown type is used for `AddOrUpdateValue`

Cheesebaron.MvxPlugins.Settings.Droid/Settings.cs
Cheesebaron.MvxPlugins.Settings.Touch/Settings.cs
Cheesebaron.MvxPlugins.Settings.WindowsPhone/Settings.cs
========================================================

- Cleanup of logic, logically does the same.

Rest of the files are boilerplate code related to [MvvmCross](https://github.com/slodge/MvvmCross)