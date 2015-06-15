Upgrade FormsPresenters to PCL Profile 259
==========================================

As of June 2015 most users of the FormsPresenters plugin will be using Visual Studio 2013. Some of us were working with the Visual Studio 2015 community technical preview
so as to gain early access to an exceptionally large release of new features. On the 29th of April Microsoft released the Visual Studio 2015 Release Candidate and there
were some changes that impacted cross platform development using Xamarin.

Prior to this watershed, when developing cross platform Apps with Xamarin, there was the iOS Target, the Android target and the Windows Phone target.

The Windows Phone target was actually targeting Silverlight 8 on Windows Phone 8. Of course for almost a year most Windows phones have been upgraded to run Windows Phone
8.1. Not really an issue because the Silverlight 8 Apps would run on WP 8.1.

Changes Starting with Visual Studio 2015 RC
===========================================

Microsoft and Xamarin had been cooperating for some time in order to get what is known as Universal Apps running on Windows 8.1 and on WP 8.1 natively i.e. no Silverlight.

Universal Apps will be a big feature in Windows 10 to be released late July. The idea is that you can go to the store and download an App and it should be able to run on
any Windows device. In order to achieve that, Microsoft needed to unify the various versions of the operating system to present a common interface between operating 
system and App. More importantly from a development perspective, they needed to provide a way for Apps to be written to accept input from different input devices and to
adapt the visual presentation dependant on display size. One of the aspects was an extension to the XAML schema that allows visual elements to adapt to screen geometry.

So starting with the upgrade to Visual Studio 2015 we discovered the following:
- Projects that targeted Windows Phone were broken and changes were needed to get them working again.
- There was an additional target you could add to your App. This target was a Universal App that would run both on WP8/8.1 natively and Windows 8/8.1.

The reason why the existing Windows Phone projects broke was because in VS 2015 RC Microsoft removed the Windows Phone 8.0 SDK. This makes sense because they provide the
Windows Phone 8.1 SDK and the 8.0 SDK is a legacy they do not want to support. I tried to fix this problem by downloading and installing the 8.0 SDK, but that does not
work as the installer asks what version of VS and does not give you a choice of 2015. But you can still install it via Visual Studio 2013.

The Windows Phone SDK provides a number of DLL's that your project needs to link to, and without these your Windows Phone Xamarin Target will not compile.

Fixing this is quite easy. 

First go to your PCL project settings page:

Markup: ![PCL settings before](https://github.com/PeterBurke/Cheesebaron.MvxPlugins/blob/master/wpsettingbefore.png)

This is the **PCL profile 78** that we all have been using for some time now.

Notice the Targeting and Targets. This is a feature of the property page for a PCL project. You select a set of targets that this PCL is to be used by. This information
is then compiled into the Assembly metadata and later checked by the consumer project.

Click the Change... button and check two additional targets:

Markup: ![PCL settings after](https://github.com/PeterBurke/Cheesebaron.MvxPlugins/blob/master/wpsettingbefore.png)

This is **PCL Profile 259**.

You add the two new projects:

* Windows 8
* Windows Phone 8.1

This allows you to later create a new Windows Store Universal App as a target. Also including Windows Phone 8.1 tells Visual Studio to look for assembly references in the
Windows Phone SDK 8.1 and this also fixes the issue with the Windows Phone Silverlight 8 Xamarin target.

When selecting the PCL targets, you are not really able to check every possible combination of targets, for example you can select Windows 8.1 instead of Windows 8 but you
get Windows 8 because the the interfaces are identical, but primarily because there really is only a limited set of standard profiles to choose from. This keeps things
simple in the NuGet packages, you do not want every possible combination of targets as that would become massive.

The other thing you need to do is in your Windows Phone Xamarin project, open the settings: 

Markup: ![WP Settings after](https://github.com/PeterBurke/Cheesebaron.MvxPlugins/blob/master/TargetWP81.png)

Again this tells the build to locate Assemblies from the WP SDK 8.1 whereas previously it located them from the WP SDK 8.0.







This is **PCL Profile 259**.
There are two additional targets: Windows 8, Windows Phone 8.1. You need the Windows Phone 8.1 to reference DLL's in that SDK. You don't have the option
not to also include Windows 8.

When selecting the PCL targets, you are not really able to check every possible combination, for example you can select Windows 8.1 instead of Windows 8
but you get Windows 8 because they the interfaces are identical, but primarily because there really is only a limited set of standard profiles to choose
from. This keeps things simple in the NuGet packages, you do not want every possible combination of targets as that would become massive.

It you need to know where these Portable Profiles are documented, that actually is a hard question. I use a tool to enumerate all of the assemblies in

> C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETPortable\

Looking into the metadata in these DLL's I find 46 distinct profiles. The information relating to profile 259 is as follows:

-	"fullName": ".NETPortable,Version=v4.5,Profile=Profile259",
-	"displayName": ".NET Portable Subset (.NET Framework 4.5, Windows 8, Windows Phone 8.1, Windows Phone Silverlight 8)",
-	"profileName": "Profile259",
-	"supportedByVisualStudio2013": true,
-	...

So it would appear that switching to PCL Profile 259 should not impact developers still using Visual Studio 2013.

### Summary ###

To adapt to the changes:
-	you need to be using Visual Studio 2013 with latest updates, or using Visual Studio 2015 RC or later
-	The removal of the WP 8.0 SDK in Visual Studio 2015 means we need to be using **PCL Profile 259**
-	This profile adds two new targets: Windows 8 and Windows Phone 8.1, but you don't have to create Apps for these targets
-	Xamarin actually targets Silverlight 8 for its Windows Phone App and Silverlight 8 continues to be supported.
-	Visual Studio 2015 users will need to change the property pages of their Windows Phone App's as targeting Windows Phone 8.1 to 8.0 to get the SDK.
-	Visual Studio 2013 users should do likewise, although existing compilations may continue to work.

Recently Xamarin released their Xamarin.Forms product for Windows 8.1 and Windows Phone 8.1 (sans silverlight). There are some missing elements in the XAML
right now, but that is expected to be fixed very soon.

I personally added a Windows 8 target to a project and tested it. This kind of App would run on a Windows Tablet.

Also note for Windows 10 Microsoft have created a Universal APP and this kind of App will run on all Windows Platforms: Desktop, Tablet, Mobile, XBOX, ...
The XAML here is adaptive in that you can describe how the presentation should degrade when the App is accessed on smaller screens. This is why it is called
a Universal App. Xamarin are working on matching these developments in their own variation of XAML.

This could change the standard PCL profile yet again, but Universal Apps were actually developed for Windows 8.1 so Profile 259 is the one to use.

I would like to include Xamarin Mac here also but the Xamarin Mac product really has a different purpose and that is to build pure Mac applications 
mixing c# and Objective-C.

Planned Changes
===============

My changes then to MvxPlugins will be limited to:

1. Changing the Sample FormsPresenters to use profile 259
2. Changing the PCL profile of the FormsPresenters.Core to profile 259
3. Adding additional FormsPresenters for Windows8.
4. Adding the Samples needed to demonstrate new targets working.

Other things I am working on will be done in another Git Repository.

Tomasz has hinted that I might need to update NuGet, or possibly set up a separate NuGet Package of FormsPresenters. I will come back to that issue,
but given that a NuGet Package of FormsPresenters based on profile 259 should continue to support solutions that were developed in VS 2013 based on
PCL profile 78, this could be quite simple.


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
