#tool "nuget:?package=NUnit.ConsoleRunner"
#addin "Cake.Powershell"
#addin "Cake.XdtTransform"

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var solutionFile = GetFiles("./*.sln").First();
var solution = new Lazy<SolutionParserResult>(() => ParseSolution(solutionFile));

var dateTimeFolderName = DateTime.Now.ToString("MMddyyyy");

var ngsource = new {
	  Name = "FusionComputing",
	  SourceUrl = "http://fusioncomputing.com/fcng/nuget"
	};

Task("Config")
    .Does(() => {
		if (!NuGetHasSource(ngsource.SourceUrl))
		{
			Information("Adding nuget source...");
			NuGetAddSource(ngsource.Name,ngsource.SourceUrl);
		}
	});

Task("Restore")    
    .IsDependentOn("Config")
	.Does(() => {				
		NuGetRestore(solutionFile);
	});

Task("Build")
	.IsDependentOn("Restore")
	.Does(() => {
		MSBuild(solutionFile,
		configurator => configurator.SetConfiguration("Release")
		.SetVerbosity(Verbosity.Minimal)
		.UseToolVersion(MSBuildToolVersion.VS2017)
		.SetMSBuildPlatform(MSBuildPlatform.Automatic)
		.SetPlatformTarget(PlatformTarget.MSIL));
	});

Task("Test")
    .IsDependentOn("Build")    
	.Does(() => 
	{
		
	});

Task("Default")
	.IsDependentOn("Test");

RunTarget(target);
