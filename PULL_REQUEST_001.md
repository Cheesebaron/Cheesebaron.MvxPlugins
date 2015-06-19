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

OPEN ISSUES
===========

Tomasz Cielecki as the owner of MvxPlugins I would like some feedback from you about this change and some additional changes I have in mind. These additional changes
can be deferred to another pull request or if you take your time, I might have them ready to be released as a single new revision.

### NuGet Target Strings ###

In the NuSpec we currently have a set of Target strings and I am concerned that these do not properly align with PCL 259 Profile.

| Project     | Target String (ChheseBaron)                                    | Xamarin.Forms                                                               | MvvmCross.HotTuna.CrossCore.3.5.1 |
| ----------- | -------------------------------------------------------------- | --------------------------------------------------------------------------- | --------------------------------- |
| Portable    | portable-net45+netcore45+wpa81+wp8+MonoAndroid10+Xamarin.iOS10 | portable-win+net45+wp80+win81+wpa81+MonoAndroid10+MonoTouch10+Xamarin.iOS10 | portable-win+net45+wp8+win8+wpa81 |
| Phone (sl8) | wp8                                                            | wp80                                                                        | wp8 |
| droid       | MonoAndroid10                                                  | MonoAndroid10                                                               | MonoAndroid |
| touch       | Xamarin.iOS10                                                  | Xamarin.iOS10                                                               | Xamarin.iOS10 |
| old iOS     |                                                                | MonoTouch10                                                                 | |
| new win     |                                                                | win81                                                                       | win81 |
| win store   |                                                                | wpa81                                                                       | wpa81 |

We also have for the portable project:

| Plunker 259                  |
| ---------------------------- |
| portable-net45+netcore45+wp8 |


The Target strings define a set of folders that are under the lib folder in the package. NuGet when it is setting up packages for the first time, for
each project in the solution, must select one only of these directories and link the project to the assemblies in that folder. It also creates a
packages.config file and records details of the directories these linkages for subsequent package restore process.

The naming convention for these Target Strings is documented in [NuGet Enforced Package Conventions](https://docs.nuget.org/create/enforced-package-conventions)
but seems to be hopelessly out of date.

Further references are [Stephen Cleary](http://blog.stephencleary.com/2012/05/framework-profiles-in-net.html) and [Plunker](http://embed.plnkr.co/03ck2dCtnJogBKHJ9EjY/preview)

There is only weak consistency in the naming of NuGet Target strings in the above!

Does it matter? It should not matter once you get the packages.config files created because after then NuGet should not care how you named these directories. Plus
if you create your Solution from a Template, that Template will include packages.config files and the package restore does the rest. However Xamarin cross platform
Solutions are typically have one or more PCL projects and many targets, each target needs one or more of the PCL's and a sunset of the assemblies like Droid and iOS10
that are specific to each target.

I would like to get it right. From what I can tell by looking into the Git source for NuGet, that code is being changed very frequently, and there are files that contain
the information I need. FrameworkConstants.cs contains a list of FrameworkIdentifiers e.g. Xamarin.iOS.

DefaultPortableFrameworkMappings.cs contains a Dictionary that has the profile number as key and then lists the CommonFrameworks supported. This excludes the Xamarin frameworks,
not sure why, the Xamarin Frameworks should implement a dot net core framework on another target providing complete coverage of the .NET Framework number. Mostly that is true,
and so the Nuget developers see no point in listing Xamarin targets in addition to .NET targets.

I need to look into this further in order to understand how it works and then nail down the correct NuGet target strings.

### Update the Version of the NuSpec ###

This could help avoid problems if the NuGet Target String changes. We would include a powershell script to rewrite package.config files when upgrading so that
existing references copy over with a package restore.

### FormsPresenters NuSpec Issue Xamarin.Forms XAML Solution ###

The Sample Application for the FormsPresenters called Movies does not use the NuSpec, instead it does a project reference within a top level solution. Also this
sample uses C# to code Xamarin.Forms and not XAML. I have worked on building a set of examples that use the FormsPresenters but in each case mine are based on XAML.

If a NuGet user uses the FormsPresenters plugin, it creates code files that basically will break a XAML based solution.

For example in the NuSpec:

```
		<file src="CoreContent\FirstViewModel.cs.pp" 
			target="content\portable-net45+netcore45+wpa81+wp8+MonoAndroid10+Xamarin.iOS10\ViewModels\FirstViewModel.cs.pp" />
		<file src="CoreContent\FirstPage.cs.pp" 
			target="content\portable-net45+netcore45+wpa81+wp8+MonoAndroid10+Xamarin.iOS10\Pages\FirstPage.cs.pp" />
```

The pp files will be copied to the target directory and renamed to remove the pp extension. Plus there will be some substitutions in the code.

What I would like to do for the next Version is to remove these. Instead what I propose is to simply take the Sample application Movies and to use it to create
a Visual Studio Template. These Templates if you recall do something similar to the NuSpec substitutions enabling the developer to choose a solution other than Movies
and to locate where the solution will be deposited.

I will not attempt to do this for any of the other plugins, but it could be helpful for consistency. I will also add an additional MD file that documents
the FormsPresnter solution from the viewpoint of a developer trying to use that plugin. If you need help in getting the remaining Plugins into the same state I can help.

On my own Site I intend to setup a collection of XAML based project and item templates that cover a range of things. They will need the FormsPresenters NuGet, but
they cover a range of topics. For example usage of the new Entity Framework 7 for SQLite.

### FormsPresenter for Windows and Windows Phone no Silverlight ###

With the changes that came with VS 2015 RC, it became possible to create a new Xamarin Target project for Windows and Windows Phone. This target does not use Silverlight.
Right now though when you create a Xamarin Solution using Xamarin's template, they do not automatically add this target.

As previously mentioned, this target is really a Windows Store App that can run on every Windows device, so long as they are running Windows 8.1, Windows Phone 8.1
or Windows 10. That requires many enhancements to the Xamarin.Forms Schema, but right now for this target Xamarin do not completely implement everything that is in
the original schema.

I can write the FormsPresenter and sample component for this target.


