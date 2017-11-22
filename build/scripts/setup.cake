///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(context =>
{
    // Executed BEFORE the first task.
    Information("Building version {0} of {1}.", semVersion, appName);
    Information("Target: {0}.", target);
    Information("Tools dir: {0}.", tools);
    Information("Username:  {0}", username);
});



Teardown(context =>
{
    // Executed AFTER the last task.
    Information("Finished building version {0} of {1}.", semVersion, appName);
});
