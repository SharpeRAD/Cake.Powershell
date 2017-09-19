///////////////////////////////////////////////////////////////////////////////
// PACKAGE
///////////////////////////////////////////////////////////////////////////////

Task("Copy-Files")
    .Does(() =>
{
    if (projectBinDirs.Count > 0)
    {
        bool found = false;

		CleanDirectory("./test/tools/Addins/");

        foreach (string project in projectBinDirs)
        {
            if (!project.Contains(".Websites") && !project.Contains(".Tests") && !project.EndsWith(".TestConsole") && !project.EndsWith(".TestSite"))
    		{
                found = true;
                CopyDirectory(project + "/" + configuration, binDir);
				CopyDirectory(project + "/" + configuration, "./test/tools/Addins/" + appName + "." + version + "/lib/");
            }
        }

        if (found)
        {
			// Docs
            CopyFiles(new FilePath[] { "LICENSE", "README.md", "ReleaseNotes.md" }, binDir);
        }
    }
});



Task("Zip-Files")
    .IsDependentOn("Copy-Files")
    .Does(() =>
{
    // Apps
    bool found = false;

    foreach (string project in projectNames)
    {
		if (project.Contains(".Websites") && !project.EndsWith(".Tests") && !project.EndsWith(".TestConsole") && !project.EndsWith(".TestSite"))
		{
            found = true;
        	Zip(binDir + Directory(project), deployDir + "/" + project + ".zip");
		}
	}

    // Libraries
    if (!found)
    {
        Zip(binDir, zipPackage);
    }
});



Task("Create-NuGet-Packages")
    .IsDependentOn("Zip-Files")
    .Does(() =>
{
    foreach (string project in projectNames)
    {
        if (!project.EndsWith(".Tests") && !project.EndsWith(".TestConsole") && !project.EndsWith(".TestSite"))
        {
            // Solution Packages
            NuGetPack("./nuspec/" + project + ".nuspec", new NuGetPackSettings
            {
                Version = version,
                ReleaseNotes = releaseNotes.Notes.ToArray(),
                BasePath = binDir,
                OutputDirectory = nugetDir,
                Symbols = false,
                NoPackageAnalysis = true
            });
        }
    }
});
