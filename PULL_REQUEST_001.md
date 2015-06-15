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

![PCL settings before](https://github.com/PeterBurke/Cheesebaron.MvxPlugins/blob/master/wpsettingbefore.png)

This is the **PCL profile 78** that we all have been using for some time now.

Notice the Targeting and Targets. This is a feature of the property page for a PCL project. You select a set of targets that this PCL is to be used by. This information
is then compiled into the Assembly metadata and later checked by the consumer project.

Click the Change... button and check two additional targets:

![PCL settings after](https://github.com/PeterBurke/Cheesebaron.MvxPlugins/blob/master/wpsettingafter.png)

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

![WP Settings after](https://github.com/PeterBurke/Cheesebaron.MvxPlugins/blob/master/TargetWP81.png)

Again this tells the build to locate Assemblies from the WP SDK 8.1 whereas previously it located them from the WP SDK 8.0.

### How Does Changing from PCL Profile 78 to 259 Impact VS 2013 Developers ###

This change should not impact VS 2013 developers. The only real change is in the metadata in the PCL. PCL 259 has been around for quite some time now. What the metadata
does is that allows the build to check the suitability of the PCL to be consumed by the Consumer. We have added two new targets, but we have not removed any existing targets.

The Android and iOS targets need to know that the PCL can be run on those platforms. If we look into the full metadata for PCL 259:

```
{
	"fullName": ".NETPortable,Version=v4.5,Profile=Profile259",
	"displayName": ".NET Portable Subset (.NET Framework 4.5, Windows 8, Windows Phone 8.1, Windows Phone Silverlight 8)",
	"profileName": "Profile259",
	"supportedByVisualStudio2013": true,
	"supportsAsync": false,
	"supportsGenericVariance": false,
	"nugetTarget": "",
	"frameworks": [{
		"fullName": ".NETFramework,Version=v4.5,Profile=*",
		"displayName": ".NET Framework"
	},
	{
		"fullName": ".NETCore,Version=v4.5,Profile=*",
		"displayName": "Windows"
	},
	{
		"fullName": "WindowsPhoneApp,Version=v8.1,Profile=*",
		"displayName": "Windows Phone"
	},
	{
		"fullName": "WindowsPhone,Version=v8.0",
		"displayName": "Windows Phone Silverlight"
	},
	{
		"fullName": "MonoAndroid,Version=v1.0,Profile=*",
		"displayName": "Xamarin.Android"
	},
	{
		"fullName": "Xamarin.iOS,Version=v1.0,Profile=*",
		"displayName": "Xamarin.iOS"
	},
	{
		"fullName": "MonoTouch,Version=v1.0,Profile=*",
		"displayName": "Xamarin.iOS (Classic)"
	}]
}
```

You can see that this profile quite clearly supports Xamarin.iOS classic and the new iPhone iPad unified targets, plus MonoAndroid. It also supports
"Windows Phone Silverlight" a target that is the same on Windows Phone 8.0 and Windows Phone 8.1.

### Conclusion ###

Switching from PCL Profile 78 to 259 in the FormsPresenters should not impact VS 2013 developers. But of course take time to test this.

The changes I have made in the Pull Request have all been tested using VS 2015 RC, i.e. the three FormsPresenters have been proven to work. I have not
tested them against VS 2013.

NUGET TARGET STRINGS
====================

The NuGet enforced package conventions describes how NuGet when called from within Visual Studio attempts to link or relink referenced assemblies from
downloaded packages directory. Often their are multiple subdirectories for it to choose from, but it only ever links a VS Project to assemblies within
a single directory. The directory naming conventions are designed to make this selection automatic.

Originally the only choices were based on dot net framework versions. After the invention of Portable Libraries, things got a lot more complex. In fact
it seems to me that there is no clear standard here.

For example in the FormsPresenters NuSpec we have for the PCL a "Nuget Target" = "lib\portable-net45+netcore45+wpa81+wp8+MonoAndroid10+Xamarin.iOS10".
So when we install the package it creates a directory tree that matches that, put the DLL's in there and links them to your project. In the project
directory, it records the package it linked to in a packages.config file. This helps NuGet restore packages. The convention is that second level packages
are not downloaded directly but only downloaded if they are not already downloaded so this makes good sense.

In this process NuGet really does not care how we name these target directories. But it may do so if you run the package manager at a solution level.

Now what I have noticed is that there is quite a lot of inconsistency between these NuGet Target Strings. Theoretically they should be the same for the
same PCL Profile, but different developers have different ideas about that.

For example if "portable-net45+netcore45+wpa81+wp8+MonoAndroid10+Xamarin.iOS10" is the string for PCL profile 259 it should be
"portable-net45+netcore45+wp8" according to [Plunker](http://embed.plnkr.co/03ck2dCtnJogBKHJ9EjY/preview).

Xamarin however for their Xamarin.forms NuGet package use "portable-win+net45+wp80+win81+wpa81+MonoAndroid10+MonoTouch10+Xamarin.iOS10" as their
NuGet string,

I Recommend leaving this as is for now, because changing it could impact existing installs.

FORMSPRESENTERS NUSPEC FOR XAMARIN FORMS XAML SOLUTION
======================================================

I need to create a NuGet package that differs slightly from the "Cheesebaron.MvxPlugins.FormsPresenters" package. This is for a XAML Based solution.
The FormsPresenters DLL's generated by this Pull request are OK, the issue is that the NuSpec on package restore is creating files that conflict with
a XAML based solution. For example FirstPage.cs is a problem here because in a XAML based solution, pages are written in XAML rather than CS.

The change is so trivial that it would be a shame to have to permanently for the Repository in order to achieve this.

I wonder if we could not simply put a second NuSpec in there with say a Package ID of "Cheesebaron.MvxPlugins.FormsPresenters.ForXaml"?

