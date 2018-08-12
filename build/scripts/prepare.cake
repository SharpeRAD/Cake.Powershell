///////////////////////////////////////////////////////////////////////////////
// PREPARE
///////////////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
	Information("Cleaning project files");

	CleanDirectories(projectBinDirs);
	CleanDirectories(projectObjDirs);



	Information("Cleaning build files");

    CleanDirectories(new DirectoryPath[]
    {
        buildResultDir,
        testResultsDir,
        nugetDir, deployDir, binDir
    });

	

	Information("Cleaning nuspec temp files");

	DeleteFiles("./nuspec/*.temp.nuspec");
});



Task("Restore-Nuget-Packages")
    .IsDependentOn("Clean")
	.WithCriteria(() => target != "Skip-Restore")
    .Does(() =>
{
    // Restore all main projects
	foreach (string project in projectNames)
	{
        if (projectDirs.Contains("./src/" + project))
        {
    		Information("Restoring {0}", project);

    		DotNetCoreRestore("./src/" + project, new DotNetCoreRestoreSettings()
            {
				Verbosity = DotNetCoreVerbosity.Normal
            });
        }
	}

	//Restoring test projects
    foreach(string project in testNames)
    {
        Information("Restoring: {0}", project);

        DotNetCoreRestore("./src/" + project, new DotNetCoreRestoreSettings()
        {
			Verbosity = DotNetCoreVerbosity.Normal
        });
    }
});
