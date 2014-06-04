MvxPlugins
==========

This repository is a collection of plugins for MvvmCross. Currently it consists of the following plugins:

- **AppId** A plugin for genererating a Unique application ID and get some basic information about the phone
- **Azure Access Control** This plugin provides Authentication against Windows Azure ACS (don't confuse it with Windows Azure Mobile Services).
- **Connectivity** A plugin for getting network information and status. [aritchie's](https://github.com/aritchie/acrmvvmcross/tree/master/Acr.MvvmCross.Plugins.Network) plugin is a great alternative to this plugin.
- **ModernHttpClient** A wrapper around [Paul Betts'][paulb] [ModernHttpClient][modern].
- **Settings** A plugin for saving simple key/value kind of settings into persistant storage
- **SimpleWebToken** A plugin to create SimpleWebToken's from raw representations and to generate your own.
- **SMS** A simple task to send SMS using default/install SMS applications on device.
- More to come! If you have a good idea, feel free to pitch it with me.

Thanks to
=========

- [Stuart Lodge][slodge] and the community for [MvvmCross][mvx]
- Microsoft for [Windows Azure Toolkits][wat] which the Azure Access Control plugin is based on.
- [James Montemagno][james] for his [Settings][ceton] plugin, which the **Settings** plugin in this repository is based on.
- [Coworkers at Brüel & Kjœr EMS](http://bksv.com) for allowing me to publicise code to generate a **SimpleWebToken**.
- [Xamarin][xam] for providing a [reachability sample][reach] for Touch projects.
- [Paul Betts][paulb] for his great work on [ModernHttpClient][modern].

Documentation
=============

For the moment look at the samples. More detailed docs will come in the Wiki.

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


License
=======

- **AppId** plugin is licensed under [Apache 2.0][apache]
- **Azure Access Control** plugin is licensed under [Apache 2.0][apache]
- **Connectivity** plugin is licensed under [Apache 2.0][apache]
- **ModernHttpClient** plugin is licensed under [Apache 2.0][apache] the assemblies used inside of it are [AFNetworking](http://afnetworking.com/), which is under the [MIT License][mit] and [OkHttp](http://square.github.io/okhttp/) which is licensed under [Apache 2.0][apache]. [ModernHttpClient][modern] which this plugin wraps is under the [following license](https://github.com/paulcbetts/ModernHttpClient/blob/master/COPYING).
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
