MvxPlugins
==========

[![Build status](https://ci.appveyor.com/api/projects/status/p22bm2v0m41lgqni?svg=true)](https://ci.appveyor.com/project/Cheesebaron/cheesebaron-mvxplugins)

| Plugin          | NuGet version                                                                                                                                                              |
| --------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Settings        | [![NuGet](https://img.shields.io/nuget/v/Cheesebaron.MvxPlugins.Settings.svg?maxAge=2592000)](https://www.nuget.org/packages/Cheesebaron.MvxPlugins.Settings/)             |
| DeviceInfo      | [![NuGet](https://img.shields.io/nuget/v/Cheesebaron.MvxPlugins.DeviceInfo.svg?maxAge=2592000)](https://www.nuget.org/packages/Cheesebaron.MvxPlugins.DeviceInfo/)         |
| Connectivity    | [![NuGet](https://img.shields.io/nuget/v/Cheesebaron.MvxPlugins.Connectivity.svg?maxAge=2592000)](https://www.nuget.org/packages/Cheesebaron.MvxPlugins.Connectivity/)     |
| SMS             | [![NuGet](https://img.shields.io/nuget/v/Cheesebaron.MvxPlugins.SMS.svg?maxAge=2592000)](https://www.nuget.org/packages/Cheesebaron.MvxPlugins.SMS/)                       |
| SimpleWebToken  | [![NuGet](https://img.shields.io/nuget/v/Cheesebaron.MvxPlugins.SimpleWebToken.svg?maxAge=2592000)](https://www.nuget.org/packages/Cheesebaron.MvxPlugins.SimpleWebToken/) |

This repository is a collection of plugins for MvvmCross. Currently it consists of the following plugins:

- **DeviceInfo** A plugin for getting information about your device, such as screen dimensions, device id, firmware version, memory, type and more.
- **Connectivity** A plugin for getting network information and status. [aritchie's](https://github.com/aritchie/acrmvvmcross/tree/master/Acr.MvvmCross.Plugins.Network) plugin is a great alternative to this plugin.
- **Settings** A plugin for saving simple key/value kind of settings into persistant storage
- **SimpleWebToken** A plugin to create SimpleWebToken's from raw representations and to generate your own.
- **SMS** A simple task to send SMS using default/install SMS applications on device.
- More to come! If you have a good idea, feel free to pitch it with me.

Thanks to
=========

- [Stuart Lodge][slodge] and the community for [MvvmCross][mvx]
- [James Montemagno][james] for his [Settings][ceton] plugin, which the **Settings** plugin in this repository is based on.
- [Coworkers at Brüel & Kjœr EMS](http://bksv.com) for allowing me to publicise code to generate a **SimpleWebToken**.
- [Xamarin][xam] for providing a [reachability sample][reach] for Touch projects.

Contributors 
============
Major contributors will be listed below.

- [Patrick Long][munkii] - WPF solution for [Settings][settings].

Documentation
=============

For the moment look at the samples. More detailed docs will come in the Wiki (when someone adds it).

Other MvvmCross plugins
=======================

Other people are doing MvvmCross plugins as well and I think it is great to mention them, as they might have a useful plugin for your MvvmCross project.

| Dev                                 | Plugin                                      |
| ----------------------------------- | ------------------------------------------- |
| [Kerry Street][kstreet]             | [Street.MvxPlugins][streetmvx]              |
| [James Montemagno][james] for ceton | [Mvx.Plugins.Settings][ceton]               |
| [Geoffrey Huntley][ghuntley]        | [Ghuntley.MvxPlugins.FaceTime][facetime]    |
| [Artur Rybak][wedkarz]              | [IHS.MvvmCross.Plugins.Keychain][keychain]  |
| [Allan Ritchie][aritchie]           | [acrmvvmcross][acrmvvmcross]                |
| [SeeD-Seifer][SeeD-Seifer]          | [Mvx.Geocoder][geocoder]                    |
| [ChristianRuiz][ChristianRuiz]      | [MvvmCross-SecureStorage][secure-storage]   |
| [ChristianRuiz][ChristianRuiz]      | [MvvmCross-ControlsNavigation][controlsnav]   |


License
=======

- **DeviceInfo** plugin is licensed under [Apache 2.0][apache]
- **Connectivity** plugin is licensed under [Apache 2.0][apache]
- **Settings** plugin is licensed under [Apache 2.0][apache]
- **SimpleWebToken** plugin is licensed under [Apache 2.0][apache]
- **SMS** plugin is licensed under [Apache 2.0][apache]

[apache]: https://www.apache.org/licenses/LICENSE-2.0.html
[mit]: http://opensource.org/licenses/mit-license
[kstreet]: https://github.com/kstreet
[streetmvx]: https://github.com/kstreet/Street.MvxPlugins
[james]: https://github.com/jamesmontemagno
[ceton]: https://github.com/ceton/Mvx.Plugins.Settings
[ghuntley]: https://github.com/ghuntley
[facetime]: https://github.com/ghuntley/Ghuntley.MvxPlugins.FaceTime
[wedkarz]: https://github.com/wedkarz
[keychain]: https://github.com/wedkarz/IHS.MvvmCross.Plugins.Keychain
[aritchie]: https://github.com/aritchie
[acrmvvmcross]: https://github.com/aritchie/acrmvvmcross
[slodge]: https://github.com/slodge
[mvx]: https://github.com/slodge/MvvmCross
[wat]: https://github.com/WindowsAzure-Toolkits
[xam]: http://xamarin.com
[modern]: https://github.com/paulcbetts/ModernHttpClient
[paulb]: https://github.com/paulcbetts
[reach]: https://github.com/xamarin/monotouch-samples/blob/master/ReachabilitySample/reachability.cs
[SeeD-Seifer]: https://github.com/SeeD-Seifer
[geocoder]: https://github.com/SeeD-Seifer/Mvx.Geocoder
[secure-storage]: https://github.com/ChristianRuiz/MvvmCross-SecureStorage
[controlsnav]: https://github.com/ChristianRuiz/MvvmCross-ControlsNavigation
[ChristianRuiz]: https://github.com/ChristianRuiz
[marcos]: https://github.com/MarcosCobena
[fp]: https://github.com/Cheesebaron/Cheesebaron.MvxPlugins/tree/master/FormsPresenters
[settings]: https://github.com/Cheesebaron/Cheesebaron.MvxPlugins/tree/master/Settings
[munkii]: https://github.com/munkii
