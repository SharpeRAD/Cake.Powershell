//////////////////////////////////////////////////////////////////////
// VARIABLES
//////////////////////////////////////////////////////////////////////

//Setup
var tools = EnvironmentVariable("CAKE_PATHS_TOOLS");
var username = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile).ToLower().Replace(@"c:\users\", "").Replace(@"c:\windows\serviceprofiles\", "");



// Get whether or not this is a local build.
var local = BuildSystem.IsLocalBuild;
var isRunningOnAppVeyor = AppVeyor.IsRunningOnAppVeyor;
var isRunningOnTFS = (EnvironmentVariable("TF_BUILD") == "True");
var isPullRequest = AppVeyor.Environment.PullRequest.IsPullRequest;

// Parse release notes.
var releaseNotes = ParseReleaseNotes("./ReleaseNotes.md");
var copyright = "Phillip Sharpe";

// Get version.
var buildNumber = AppVeyor.Environment.Build.Number;
var version = releaseNotes.Version.ToString();
var semVersion = local ? version : (version + string.Concat("-build-", buildNumber));

// Define directories.
var buildResultDir = "./build/results";
var testResultsDir = buildResultDir + "/tests";
var loggerResultsDir = buildResultDir + "/logger";
var nugetDir = buildResultDir + "/nuget";
var deployDir = buildResultDir + "/deploy";
var binDir = buildResultDir + "/bin";



// Package Location
var zipPackage = buildResultDir + "/" + appName.Replace(".", "-") + "-v" + semVersion + ".zip";

// Project Locations
var solution = "./src/" + appName + ".sln";

var projectDirs = new List<string>();
var projectBinDirs = new List<string>();
var projectObjDirs = new List<string>();
var projectFiles = new List<FilePath>();

foreach (string project in projectNames)
{
    //Exclude Legacy projects without namespaces
    if (project.Contains(".") && DirectoryExists("./src/" + project))
    {
        projectDirs.Add("./src/" + project);

		projectBinDirs.Add("./src/" + project + "/bin");
		projectObjDirs.Add("./src/" + project + "/obj");

        projectFiles.Add(File("./src/" + project + "/" + project + ".xproj"));
    }
}



//Find Tests
var testNames = new List<string>();

if (DirectoryExists("./src/" + appName + ".Tests"))
{
    testNames.Add(appName + ".Tests");

	projectBinDirs.Add("./src/" + appName + ".Tests/bin/");
	projectObjDirs.Add("./src/" + appName + ".Tests/obj");
}

foreach (string project in projectNames)
{
    if (DirectoryExists("./src/" + project + ".Tests") && !testNames.Contains(project + ".Tests"))
    {
        testNames.Add(project + ".Tests");
		
		projectBinDirs.Add("./src/" + project + ".Tests/bin");
		projectObjDirs.Add("./src/" + project + ".Tests/obj");
    }
}
