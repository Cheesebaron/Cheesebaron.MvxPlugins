#tool "nuget:?package=GitVersion.CommandLine"
#tool "nuget:?package=gitlink"

var sln = new FilePath("Cheesebaron.MvxPlugins.sln");
var binDir = new DirectoryPath("bin");
var outputDir = new DirectoryPath("artifacts");
var target = Argument("target", "Default");

var isRunningOnAppVeyor = AppVeyor.IsRunningOnAppVeyor;
var isPullRequest = AppVeyor.Environment.PullRequest.IsPullRequest;

Task("Clean").Does(() =>
{
    CleanDirectories("./**/bin");
    CleanDirectories("./**/obj");
	CleanDirectories(binDir.FullPath);
	CleanDirectories(outputDir.FullPath);
});

GitVersion versionInfo = null;
Task("Version").Does(() => {
	GitVersion(new GitVersionSettings {
		UpdateAssemblyInfo = true,
		OutputType = GitVersionOutput.BuildServer
	});

	versionInfo = GitVersion(new GitVersionSettings{ OutputType = GitVersionOutput.Json });
	Information("VI:\t{0}", versionInfo.FullSemVer);
});

Task("Restore").Does(() => {
	NuGetRestore(sln);
});

Task("Build")
	.IsDependentOn("Clean")
	.IsDependentOn("Version")
	.IsDependentOn("Restore")
	.Does(() =>  {
	
	DotNetBuild(sln, 
		settings => settings.SetConfiguration("Release")
	);
});

Task("GitLink")
	.IsDependentOn("Build")
	.Does(() => {

	if (IsRunningOnWindows()) //pdbstr.exe and costura are not xplat currently
		GitLink(sln.GetDirectory(), new GitLinkSettings {
			RepositoryUrl = "https://github.com/Cheesebaron/Cheesebaron.MvxPlugins",
			ArgumentCustomization = args => args.Append(
				"-ignore Sample,sms.sample.core,sms.sample.droid,sms.sample.touch,settings.sample.core,settings.sample.droid,settings.sample.windowsphone,settings.sample.touch,settingssample.windowscommon.core,connectivitysample.core,connectivitysample.touch,settingssample.windowscommon.windows,settingssample.windowscommon.windowsphone,connectivitysample.store.windows,connectivitysample.store.windowsphone,connectivitysample.droid")
		});
});

Task("PackageAll")
	.IsDependentOn("GitLink")
	.Does(() => {

	EnsureDirectoryExists(outputDir);

	var nugetSettings = new NuGetPackSettings {
		Authors = new [] { "Tomasz Cielecki" },
		Owners = new [] { "Tomasz Cielecki" },
		IconUrl = new Uri("http://i.imgur.com/V3983YY.png"),
		ProjectUrl = new Uri("https://github.com/Cheesebaron/Cheesebaron.MvxPlugins"),
		LicenseUrl = new Uri("https://github.com/Cheesebaron/Cheesebaron.MvxPlugins/blob/master/LICENSE"),
		Copyright = "Copyright (c) Tomasz Cielecki",
		RequireLicenseAcceptance = false,
		ReleaseNotes = ParseReleaseNotes("./releasenotes/settings.md").Notes.ToArray(),
		Version = versionInfo.NuGetVersion,
		Symbols = false,
		NoPackageAnalysis = true,
		OutputDirectory = outputDir,
		Verbosity = NuGetVerbosity.Detailed,
		BasePath = "./nuspec"
	};

	NuGetPack("./nuspec/Cheesebaron.MvxPlugins.Settings.nuspec", nugetSettings);

	nugetSettings.ReleaseNotes = ParseReleaseNotes("./releasenotes/connectivity.md").Notes.ToArray();
	NuGetPack("./nuspec/Cheesebaron.MvxPlugins.Connectivity.nuspec", nugetSettings);

	nugetSettings.ReleaseNotes = ParseReleaseNotes("./releasenotes/deviceinfo.md").Notes.ToArray();
	NuGetPack("./nuspec/Cheesebaron.MvxPlugins.DeviceInfo.nuspec", nugetSettings);

	nugetSettings.ReleaseNotes = ParseReleaseNotes("./releasenotes/simplewebtoken.md").Notes.ToArray();
	NuGetPack("./nuspec/Cheesebaron.MvxPlugins.SimpleWebToken.nuspec", nugetSettings);

	nugetSettings.ReleaseNotes = ParseReleaseNotes("./releasenotes/sms.md").Notes.ToArray();
	NuGetPack("./nuspec/Cheesebaron.MvxPlugins.Sms.nuspec", nugetSettings);
});

Task("UploadAppVeyorArtifact")
	.IsDependentOn("PackageAll")
	.WithCriteria(() => !isPullRequest)
	.WithCriteria(() => isRunningOnAppVeyor)
	.Does(() => {

	Information("Artifacts Dir: {0}", outputDir.FullPath);

	foreach(var file in GetFiles(outputDir.FullPath + "/*")) {
		Information("Uploading {0}", file.FullPath);
		AppVeyor.UploadArtifact(file.FullPath);
	}
});

Task("Default")
	.IsDependentOn("UploadAppVeyorArtifact")
	.Does(() => {});

RunTarget(target);