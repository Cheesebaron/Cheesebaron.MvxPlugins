MvxPlugins
==========

This repository is a collection of plugins for MvvmCross. It is a fork of https://github.com/Cheesebaron/Cheesebaron.MvxPlugins by Tomasz Cielecki.

I will be pulling updates from the root on a regular basis. The intention is that this fork is temporary and that Tomasz will eventually update the
root with changes I make.


Changes should be limited to the **FormsPresenters** plugin plus examples.

The need for change occurred after the release of Visual Studio 2015 RC. Microsoft have dropped default support for Xamarin Windows Phone 8.0 and
have at long last added support for Xamarin Windows Phone 8.1 and Windows 8.1. If you had solutions that were developed via a Visual Studio version
prior to the 2015 RC then you will need to make some changes as outlined below.

Many users of MvxPlugins will still be using Visual Studio 2013 and we do not wish to disrupt their workings. For this reason there may be some delay
before Tomasz pulls the changes back into the root.

Changes Starting with Visual Studio 2015 RC
===========================================

Originally in a visual Studio Xamarin solution including FormsPresenters there were just three targets: iOS, Android and Windows Phone 8. Most Windows
Phones are actually running WP 8.1, however this was not a problem because in fact the real target was SilverLight 8 and applications written for
SilverLight 8 run on Windows Phone 8.1. At that time there was no support for Xamarin targeting Windows 8.1 or Windows Phone 8.1.

In Visual Studio 2015 RC Microsoft made several changes that require you to modify your solution.
- They removed the Windows Phone 8.0 SDK, but provide a Windows Phone 8.1 SDK. This will break your WP project. You can work around the issue by
  installing the SDK from within VS 2013. However it is easier to upgrade the project to Windows 8.1.
- They added Xamarin support for Windows 8.1 as a Target. This changes the Portable Library Profile you need you will use in your solution.

FormsPresenters project has a Portable library, and the example projects "Movies" also use a PCL

The FormsPresenters.Core project (Portable) had the following targets
* .NET Framework 4.5,
* Windows Phone Silverlight 8
* Xamarin.Android
* Xamarin.iOS
* Xamarin.iOS (Classic)

This is the **PCL profile 78** that we all have been using for some time now. It is a list of supported solution targets compiled into the Meta Data.
When you reference a PCL, VS checks that the Target you are working on is supported.

Now in the Samples Movies project (Portable), to get this working again in Visual Studio 2015 RC we need the following targets:
* .NET Framework 4.5,
* **Windows 8**
* **Windows Phone 8.1**
* Windows Phone Silverlight 8
* Xamarin.Android
* Xamarin.iOS
* Xamarin.iOS (Classic)

This is **PCL Profile 259**.
There are two additional targets: Windows 8, Windows Phone 8.1. You need the Windows Phone 8.1 to reference DLL's in that SDK. You don't have the option
not to also include Windows 8.

When selecting the PCL targets, you are not really able to check every possible combination, for example you can select Windows 8.1 instead of Windows 8
but you get Windows 8 because they the interfaces are identical, but primarily because there really is only a limited set of standard profiles to choose
from. This keeps things simple in the NuGet packages, you do not want every possible combination of targets as that would become massive.

It you need to know where these Portable Profiles are documented, that actually is a hard question. I use a tool to enumerate all of the assemblies in

> C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETPortable\v4.5

looking into their metadata and I find 46 distinct profiles. The information relating to profile 259 is as follows:
-"fullName": ".NETPortable,Version=v4.5,Profile=Profile259",
-"displayName": ".NET Portable Subset (.NET Framework 4.5, Windows 8, Windows Phone 8.1, Windows Phone Silverlight 8)",
-"profileName": "Profile259",
-"supportedByVisualStudio2013": true,
-...

So it would appear that switching to Profile 259 should not impact developers still using Visual Studio 2013.


In summary: the removal of the WP 8.0 SDK means you need to base everything on a new PCL profile that now also includes Windows 8 and Windows Phone 8.1
as Targets. You don't have add the additional Windows 8 target as a project to your solution though.

You will need to change your Windows Phone project from Windows Phone 8.0 to 8.1.

I personally added a Windows 8 target to a project and tested it. This kind of App would run on a Windows Tablet.

Also note there is yet another coming shortly. For Windows 10 Microsoft have created a Universal APP and this kind of App will run on all Windows
Platforms: Desktop, Tablet, Mobile, XBOX, ... This will change the standard PCL profile yet again.

As an aside: Windows 10 arrives late July and Microsoft are offering free upgrades to Windows 7 and Windows 8 users. I believe that the uptake will be
very large for the personal users. The enterprises should follow within the year. It is of course a cost for them, but it will also be a cost not to
upgrade because Windows 7 support ends unless the enterprise pays for extended support. Many enterprises will also be looking at developing Xamarin
multi-target Apps with the three targets being: Windows UAP, iOS, Android. I would like to include Xamarin Mac here also but the Xamarin Mac product
really has a different purpose and that is to build pure Mac applications mixing c# and Objective-C.

Planned Changes
===============

My changes then to MvxPlugins will be limited to:

1. Changing the Sample FormsPresenters to use a fully supported PCL profile
2. If required also changing the PCL profile of the FormsPresenters.Core
3. Adding additional FormsPresenters for Windows8 and Windows 10 UAP.
4. Adding the Samples needed to demonstrate new targets working.

Other things I am working on will be done in another Git Repository.

Tomasz has hinted that I might need to update NuGet, or possibly set up a separate NuGet Package of FormsPresenters. I will come back to that issue,
but given that I am only adding additional supported targets, this could be quite simple. The changed PCL Profile does not remove support
for any target, it simply adds support for additional targets, so we might be able to get away with a single set of NuGet packages.


License
=======

- **AppId** plugin is licensed under [Apache 2.0][apache]
- **Connectivity** plugin is licensed under [Apache 2.0][apache]
- **Notifications** plugin is licensed under [Apache 2.0][apache]
- **Settings** plugin is licensed under [Apache 2.0][apache]
- **SimpleWebToken** plugin is licensed under [Apache 2.0][apache]
- **SMS** plugin is licensed under [Apache 2.0][apache]
- **FormsPresenters** plugin is licensed under [MIT][mit]

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
