#addin nuget:?package=Cake.Powershell&loaddependencies=true

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");





///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(context =>
{
    //Executed BEFORE the first task.
    Information("Tools dir: {0}.", EnvironmentVariable("CAKE_PATHS_TOOLS"));
});





///////////////////////////////////////////////////////////////////////////////
// TASK DEFINITIONS
///////////////////////////////////////////////////////////////////////////////

Task("Powershell-Script")
    .Description("Run an example powershell script")
    .Does(() =>
    {
        Information("Calling Powershell Script");

        StartPowershellScript("Get-Process", new PowershellSettings()
            .SetFormatOutput()
            .SetLogOutput());

        StartPowershellScript("Stop-Service", new PowershellSettings()
            .WithArguments(args =>
            {
                args.AppendQuoted("MpsSvc");
            }));
    });

Task("Powershell-File")
    .Description("Run an example powershell file")
    .Does(() =>
    {
        Information("Calling Powershell File");

        var resultCollection = StartPowershellFile("./test.ps1");

        var returnCode = int.Parse(resultCollection[0].BaseObject.ToString());
        Information("Result: {0}", returnCode);
    });

Task("Failing-Powershell-File")
    .Description("An example showing how to handle errors from Powershell scripts")
    .Does(() =>
    {
        Information("Calling failing Powershell File");

        var resultCollection = StartPowershellFile("./failingScript.ps1");

        var returnCode = int.Parse(resultCollection[0].BaseObject.ToString());
        Information("Result: {0}", returnCode);

        if (returnCode != 0) {
            throw new ApplicationException("Script failed to execute");
        }
    });





//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Powershell-Script")
    .IsDependentOn("Powershell-File")
    .IsDependentOn("Failing-Powershell-File");





///////////////////////////////////////////////////////////////////////////////
// EXECUTION
///////////////////////////////////////////////////////////////////////////////

RunTarget(target);
