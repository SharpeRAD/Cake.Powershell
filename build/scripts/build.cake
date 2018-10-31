///////////////////////////////////////////////////////////////////////////////
// BUILD
///////////////////////////////////////////////////////////////////////////////

Task("Patch-Assembly-Info")
    .IsDependentOn("Restore-Nuget-Packages")
    .Does(() =>
{
	// Create Solution Info
    var file = "./src/SolutionInfo.cs";

    CreateAssemblyInfo(file, new AssemblyInfoSettings
    {
        Version = version,
        FileVersion = version,
        InformationalVersion = semVersion,
        Copyright = "Copyright (c) 2015 - " + DateTime.Now.Year.ToString() + " " + copyright
    });



	// Copy Solution Info
    foreach (string project in projectDirs)
    {
        CopyFileToDirectory(file, project + "/Properties");
    }
});



Task("Build")
    .IsDependentOn("Patch-Assembly-Info")
    .Does(() =>
{
    Information("Building {0}", solution);

	// Check Logger folder
	if (!DirectoryExists(loggerResultsDir))
	{
		CreateDirectory(loggerResultsDir);
	}

	// Create build settings
	var buildSettings = new DotNetCoreMSBuildSettings
	{
		Verbosity = DotNetCoreVerbosity.Normal,
		TreatAllWarningsAs = Cake.Common.Tools.DotNetCore.MSBuild.MSBuildTreatAllWarningsAs.Error,

		MaxCpuCount = 3
	};
	
	// Add Logger
	buildSettings.AddFileLogger(new MSBuildFileLoggerSettings
	{
		LogFile = loggerResultsDir + "/build.txt",

		AppendToLogFile = false,
		ShowTimestamp = false,
		ShowCommandLine = false,
		ShowEventId = false,
		ForceNoAlign = true,
		HideItemAndPropertyList = true,

		Verbosity = DotNetCoreVerbosity.Minimal
	});



	// Build Solution
	Information("Building {0}", solution);

	DotNetCoreBuild(solution, new DotNetCoreBuildSettings
    {
    	Configuration = configuration,
		
		NoRestore = true,
		MSBuildSettings = buildSettings
    });
})
.OnError(exception =>
{
	List<SlackChatMessageAttachment> attachments = new List<SlackChatMessageAttachment>();



	// Get MsBuild Errors
	var path = loggerResultsDir + "/build.txt";

	if (FileExists(path))
	{
		IList<SlackChatMessageAttachment> lstAttachments = GetMsBuildAttachments(path, exception);

		CombineAttachments(attachments, lstAttachments);
	}



	// Resolve the API key.
    var token = EnvironmentVariable("SLACK_TOKEN");

    if (string.IsNullOrEmpty(token))
    {
        throw new InvalidOperationException("Could not resolve Slack token.");
    }



	// Post Message
	SlackChatMessageSettings settings = new SlackChatMessageSettings()
	{
		Token = token,
		UserName = "Cake",
		IconUrl = new System.Uri("https://raw.githubusercontent.com/cake-build/graphics/master/png/cake-small.png")
	};

	var title = "Build failed for " + appName + " v" + version;

	SlackChatMessageResult result = Slack.Chat.PostMessage("#code", title, attachments, settings);

	
	
	// Check Result
    if (result.Ok)
    {
        // Posted
        Information("Message was succcessfully sent to Slack.");
    }
    else
    {
        // Error
        Error("Failed to send message to Slack: {0}", result.Error);
    }

	throw exception;
});