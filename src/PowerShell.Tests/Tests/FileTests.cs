#region Using Statements
    using System;
    using System.IO;
    using System.Collections.ObjectModel;

    using Xunit;

    using System.Management.Automation;
    using System.Management.Automation.Runspaces;

    using Cake.Core.Diagnostics;
    using Cake.Core.IO;
    using Cake.Powershell;
#endregion



namespace Cake.Powershell.Tests
{
    public class FileTests
    {
        [Fact]
        public void File_Local()
        {
            Collection<PSObject> results = CakeHelper.CreatePowershellRunner().Start(new FilePath("../../Scripts/Test.ps1"), 
                new PowershellSettings());

            Assert.True((results != null) && (results.Count > 0), "Check Rights");
        }



        [Fact]
        public void File_Parameters()
        {
            Collection<PSObject> results = CakeHelper.CreatePowershellRunner().Start(new FilePath("../../Scripts/Test.ps1"),
                new PowershellSettings().WithArguments(args => args.Append("Service", "eventlog")));

            Assert.True((results != null) && (results.Count == 1), "Check Rights");
        }



        [Fact]
        public void File_Remote()
        {
            Collection<PSObject> results = CakeHelper.CreatePowershellRunner().Start(new FilePath(@"C:\Test.ps1"), 
                new PowershellSettings()
                {
                    WorkingDirectory = @"C:\",
                    ComputerName = "srv-web-mon"
                });

            Assert.True((results != null) && (results.Count > 0), "Check Rights");
        }
    }
}
