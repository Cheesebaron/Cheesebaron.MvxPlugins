MvxPlugins
==========

This repository is a collection of plugins for MvvmCross. Currently it consists of the following plugins:

- **AppId** A plugin for genererating a Unique application ID and get some basic information about the phone
- **Azure Access Control** This plugin provides Authentication against Windows Azure ACS (don't confuse it with Windows Azure Mobile Services).
- **Connectivity** A plugin for getting network information and status.
- **ModernHttpClient** A wrapper around [Paul Betts's ModernHttpClient](https://github.com/paulcbetts/ModernHttpClient).
- **Settings** A plugin for saving simple key/value kind of settings into persistant storage
- **SimpleWebToken** A plugin to create SimpleWebToken's from raw representations and to generate your own.
- **SMS** A simple task to send SMS using default/install SMS applications on device.
- More to come! If you have a good idea, feel free to pitch it with me.

Thanks to
=========

- [Stuart Lodge](https://github.com/slodge) and the community for [MvvmCross](https://github.com/slodge/MvvmCross)
- Microsoft for [Windows Azure Toolkits](https://github.com/WindowsAzure-Toolkits) which the Azure Access Control plugin is based on.
- [James Montemagno](https://github.com/jamesmontemagno) for his [Settings](https://github.com/ceton/Mvx.Plugins.Settings) plugin, which the **Settings** plugin in this repository is based on.
- [Coworkers at Brüel & Kjœr EMS](http://bksv.com) for allowing me to publicise code to generate a **SimpleWebToken**.
- [Xamarin](http://xamarin.com) for providing a [reachability sample](https://github.com/xamarin/monotouch-samples/blob/master/ReachabilitySample/reachability.cs) for Touch projects.
- [Paul Bett's](https://github.com/paulcbetts) for his great work on [ModernHttpClient](https://github.com/paulcbetts/ModernHttpClient).

Documentation
=============

For the moment look at the samples. More detailed docs will come in the Wiki.

Other MvvmCross plugins
=======================

Other people are doing MvvmCross plugins as well and I think it is great to mention them, as they might have a useful plugin for your MvvmCross project.

| Dev        | Plugin           |
| ------------- |-------------|
| [Kerry Street](https://github.com/kstreet)                       | [Street.MvxPlugins](https://github.com/kstreet/Street.MvxPlugins)|
| [James Montemagno](https://github.com/jamesmontemagno) for ceton | [Mvx.Plugins.Settings](https://github.com/ceton/Mvx.Plugins.Settings) |
| [Geoffrey Huntley](https://github.com/ghuntley)                  | [Ghuntley.MvxPlugins.FaceTime](https://github.com/ghuntley/Ghuntley.MvxPlugins.FaceTime)     |
| [Artur Rybak](https://github.com/wedkarz)                        | [IHS.MvvmCross.Plugins.Keychain](https://github.com/wedkarz/IHS.MvvmCross.Plugins.Keychain) |
| [Allan Ritchie](https://github.com/aritchie)                     | [acrmvvmcross](https://github.com/aritchie/acrmvvmcross) |


License
=======

- **AppId** plugin is licensed under [Apache 2.0](https://www.apache.org/licenses/LICENSE-2.0.html)
- **Azure Access Control** plugin is licensed under [Apache 2.0](https://www.apache.org/licenses/LICENSE-2.0.html)
- **Connectivity** plugin is licensed under [Apache 2.0](https://www.apache.org/licenses/LICENSE-2.0.html)
- **ModernHttpClient** plugin is licensed under [Apache 2.0](https://www.apache.org/licenses/LICENSE-2.0.html) the assemblies used inside of it are [AFNetworking](http://afnetworking.com/), which is under the [MIT License](http://opensource.org/licenses/mit-license) and [OkHttp](http://square.github.io/okhttp/) which is licensed under [Apache 2.0](https://www.apache.org/licenses/LICENSE-2.0.html). [ModernHttpClient](https://github.com/paulcbetts/ModernHttpClient) which this plugin wraps is under the [following license](https://github.com/paulcbetts/ModernHttpClient/blob/master/COPYING).
- **Settings** plugin is licensed under [Apache 2.0](https://www.apache.org/licenses/LICENSE-2.0.html)
- **SimpleWebToken** plugin is licensed under [Apache 2.0](https://www.apache.org/licenses/LICENSE-2.0.html)
- **SMS** plugin is licensed under [Apache 2.0](https://www.apache.org/licenses/LICENSE-2.0.html)
