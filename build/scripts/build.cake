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
	// Check Logger folder
	if (!DirectoryExists(loggerResultsDir))
	{
		CreateDirectory(loggerResultsDir);
	}

	// Create build settings
	var buildSettings = new DotNetCoreMSBuildSettings
	{
		Verbosity = DotNetCoreVerbosity.Normal
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

		MSBuildSettings = buildSettings
    });
})
.OnError(exception =>
{
	// Get Errors
	string[] lines = FileReadLines(loggerResultsDir + "/build.txt");

	IList<string> errors = new List<string>();

	foreach(string line in lines)
	{
		bool found = false;

		foreach(string name in projectNames)
		{
			if (line.StartsWith("  " + name + " -> "))
			{
				found = true;
			}
		}

		foreach(string name in testNames)
		{
			if (line.StartsWith("  " + name + " -> "))
			{
				found = true;
			}
		}

		if (!found)
		{
			errors.Add(line);
		}
	}

	if (errors.Count == 0)
	{
		errors.Add(exception.Message);
	}



    // Get Message
	var title = "Build failed for " + appName + " v" + version;
    var text = "";

	for (int index = 0; index < errors.Count; index++)
	{
		text += errors[index];

		if (index < (errors.Count - 1))
		{
			text += "\n";
		}
	}



	// Resolve the API key.
    var token = EnvironmentVariable("SLACK_TOKEN");

    if (string.IsNullOrEmpty(token))
    {
        throw new InvalidOperationException("Could not resolve Slack token.");
    }



	// Post Message
	SlackChatMessageResult result;
	
	SlackChatMessageSettings settings = new SlackChatMessageSettings()
	{
		Token = token,
		UserName = "Cake",
		IconUrl = new System.Uri("https://raw.githubusercontent.com/cake-build/graphics/master/png/cake-small.png")
	};

	IList<SlackChatMessageAttachment> attachments = new List<SlackChatMessageAttachment>();
		
	attachments.Add(new SlackChatMessageAttachment()
	{
		Color = "danger",
		Text = text
	});
		
	result = Slack.Chat.PostMessage("#code", title, attachments, settings);

	
	
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