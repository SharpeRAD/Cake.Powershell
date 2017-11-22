///////////////////////////////////////////////////////////////////////////////
// PUBLISH
///////////////////////////////////////////////////////////////////////////////

Task("Clear-AppData")
    .IsDependentOn("Create-NuGet-Packages")
    .WithCriteria(() => local)
    .WithCriteria(() => !isPullRequest)
    .Does(() =>
{
    var dataDir = Context.FileSystem.GetDirectory(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"/.nuget/packages/");

    if (dataDir.Exists)
    {
        //Delete
        foreach (string project in projectNames)
        {
            var dirs = dataDir.GetDirectories(project, SearchScope.Current);
            foreach(IDirectory dir in dirs)
            {
                DeleteDirectory(dir.Path, true);
            }
        }
    }
});



Task("Publish-Local")
    .IsDependentOn("Clear-AppData")
    .WithCriteria(() => local)
    .WithCriteria(() => !isPullRequest)
    .Does(() =>
{
    var packageDir = Context.FileSystem.GetDirectory(@"C:/Projects/Packages/");

    if (packageDir.Exists)
    {
        //Delete
        foreach (string project in projectNames)
        {
            var files = packageDir.GetFiles(project + ".*", SearchScope.Current);
            foreach (IFile file in files)
            {
                DeleteFile(file.Path);
            }
        }

        //Copy
        CopyDirectory(nugetDir, packageDir.Path);
    }
});



Task("Publish-Nuget")
    .IsDependentOn("Create-NuGet-Packages")
    .WithCriteria(() => isRunningOnAppVeyor || isRunningOnTFS || (target == "Skip-Restore") )
    .WithCriteria(() => !isPullRequest)
    .Does(() =>
{
    foreach (string project in projectNames)
    {
		if (!project.EndsWith(".Tests") && !project.EndsWith(".TestConsole") && !project.EndsWith(".TestSite"))
		{
			// Check the API key
			var apiKey = EnvironmentVariable("NUGET_API_KEY");

			if (string.IsNullOrEmpty(apiKey))
			{
				throw new InvalidOperationException("Could not resolve nuget API key.");
			}

		
		
			// Push the packages
			var package = nugetDir + "/" + project + "." + version + ".nupkg";

			NuGetPush(package, new NuGetPushSettings
			{
				Source = "https://www.nuget.org/api/v2/package",
				ApiKey = apiKey
			});
		}
    }
});