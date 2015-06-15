del *.nupkg
nuget setapikey ---
nuget pack Cheesebaron.MvxPlugins.AppId.nuspec
nuget pack Cheesebaron.MvxPlugins.Sms.nuspec
nuget pack Cheesebaron.MvxPlugins.Settings.nuspec
nuget pack Cheesebaron.MvxPlugins.SimpleWebToken.nuspec
nuget pack Cheesebaron.MvxPlugins.FormsPresenters.nuspec
nuget pack Cheesebaron.MvxPlugins.Notifications.nuspec

nuget pack Cheesebaron.MvxPlugins.AppId.nuspec -symbols
nuget pack Cheesebaron.MvxPlugins.Sms.nuspec -symbols
nuget pack Cheesebaron.MvxPlugins.Settings.nuspec -symbols
nuget pack Cheesebaron.MvxPlugins.SimpleWebToken.nuspec -symbols
nuget pack Cheesebaron.MvxPlugins.FormsPresenters.nuspec -symbols
nuget pack Cheesebaron.MvxPlugins.Notifications.nuspec -symbols

nuget push Cheesebaron.MvxPlugins.AppId.1.0.2.nupkg
nuget push Cheesebaron.MvxPlugins.Sms.1.1.0.nupkg
nuget push Cheesebaron.MvxPlugins.Settings.1.2.0.nupkg
nuget push Cheesebaron.MvxPlugins.SimpleWebToken.1.0.1.nupkg
nuget push Cheesebaron.MvxPlugins.FormsPresenters.0.0.2.nupkg
nuget push Cheesebaron.MvxPlugins.Notifications.1.0.1.nupkg

nuget push Cheesebaron.MvxPlugins.AppId.1.0.2.symbols.nupkg
nuget push Cheesebaron.MvxPlugins.Sms.1.1.0.symbols.nupkg
nuget push Cheesebaron.MvxPlugins.Settings.1.2.0.symbols.nupkg
nuget push Cheesebaron.MvxPlugins.SimpleWebToken.1.0.1.symbols.nupkg
nuget push Cheesebaron.MvxPlugins.FormsPresenters.0.0.2.symbols.nupkg
nuget push Cheesebaron.MvxPlugins.Notifications.1.0.1.symbols.nupkg
pause