#tool nuget:?package=GitVersion.CommandLine&version=4.0.0
#tool nuget:?package=vswhere&version=2.6.7

#addin nuget:?package=Cake.Figlet&version=1.3.0

var sln = new FilePath("./Cheesebaron.MvxPlugins.sln");
var outputDir = new DirectoryPath("./artifacts");
var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var verbosityArg = Argument("verbosity", "Minimal");
var verbosity = Verbosity.Minimal;

var isRunningInVSTS = 
	TFBuild.IsRunningOnAzurePipelinesHosted ||
    TFBuild.IsRunningOnAzurePipelines;

GitVersion versionInfo = null;
Setup(context => 
{
    versionInfo = context.GitVersion(new GitVersionSettings {
        UpdateAssemblyInfo = true,
        OutputType = GitVersionOutput.Json
    });

    if (isRunningInVSTS) 
    {
        var buildNumber = TFBuild.Environment.Build.Number;
        TFBuild.Commands.UpdateBuildNumber(versionInfo.InformationalVersion
            + "-" + buildNumber);
    }

    var cakeVersion = typeof(ICakeContext).Assembly.GetName().Version.ToString();

    Information(Figlet("MvxPlugins"));
    Information("Building version {0}, ({1}, {2}) using version {3} of Cake.",
        versionInfo.SemVer,
        configuration,
        target,
        cakeVersion);

    verbosity = (Verbosity) Enum.Parse(typeof(Verbosity), verbosityArg, true);
});

Task("Clean")
	.Does(() =>
{
    CleanDirectories("./**/bin");
    CleanDirectories("./**/obj");
	CleanDirectories(outputDir.FullPath);

	EnsureDirectoryExists(outputDir);
});

FilePath msBuildPath;
Task("ResolveBuildTools")
    .WithCriteria(() => IsRunningOnWindows())
    .Does(() => 
{
    var vsWhereSettings = new VSWhereLatestSettings
    {
        IncludePrerelease = true,
        Requires = "Component.Xamarin"
    };
    
    var vsLatest = VSWhereLatest(vsWhereSettings);
    msBuildPath = (vsLatest == null)
        ? null
        : vsLatest.CombineWithFilePath("./MSBuild/Current/Bin/MSBuild.exe");

    if (msBuildPath != null)
        Information("Found MSBuild at {0}", msBuildPath.ToString());
});

Task("Restore")
    .IsDependentOn("ResolveBuildTools")
    .Does(() => 
{
    var settings = GetDefaultBuildSettings()
        .WithTarget("Restore");
    MSBuild(sln, settings);
});

Task("Build")
    .IsDependentOn("ResolveBuildTools")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .Does(() => 
{

    var settings = GetDefaultBuildSettings()
        .WithProperty("Version", versionInfo.SemVer)
        .WithProperty("PackageVersion", versionInfo.SemVer)
        .WithProperty("InformationalVersion", versionInfo.InformationalVersion)
        .WithProperty("NoPackageAnalysis", "True")
        .WithTarget("Build");
	
    MSBuild(sln, settings);
});


Task("CopyPackages")
    .IsDependentOn("Build")
    .Does(() => 
{
    var nugetFiles = GetFiles("./**/bin/" + configuration + "/**/*.nupkg");
    CopyFiles(nugetFiles, outputDir);
});

Task("Default")
	.IsDependentOn("CopyPackages")
	.Does(() => {});

RunTarget(target);

MSBuildSettings GetDefaultBuildSettings()
{
    var settings = new MSBuildSettings 
    {
        Configuration = configuration,
        ToolPath = msBuildPath,
        Verbosity = verbosity,
        ArgumentCustomization = args => args.Append("/m"),
        ToolVersion = MSBuildToolVersion.VS2019
    };
	
    if (isRunningInVSTS && IsRunningOnWindows())
    {
        msBuildSettings = settings.WithProperty("JavaSdkDirectory", @"C:/Program Files/Java/zulu-8-azure-jdk_8.38.0.13-8.0.212-win_x64");
    }

    return settings;
}
