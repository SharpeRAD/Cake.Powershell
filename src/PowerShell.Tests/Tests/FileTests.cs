#region Using Statements
using System.Linq;
using System.Collections.ObjectModel;
using System.Management.Automation;

using Xunit;
using Cake.Core.IO;
#endregion



namespace Cake.Powershell.Tests
{
    public class FileTests
    {
        #region Methods (4)
        [Fact]
        public void Should_Start_Local_File()
        {
            Collection<PSObject> results = CakeHelper.CreatePowershellRunner().Start(new FilePath("../../Scripts/Test.ps1"), 
                new PowershellSettings());

            Assert.True((results != null) && (results.Count > 0), "Check Rights");
        }



        [Fact]
        public void Should_Start_File_With_Parameters()
        {
            Collection<PSObject> results = CakeHelper.CreatePowershellRunner().Start(new FilePath("../../Scripts/Test.ps1"),
                new PowershellSettings().WithArguments(args => args.Append("Service", "eventlog")));

            Assert.True((results != null) && (results.Count >= 1), "Check Rights");
        }

        [Fact]
        public void Should_Return_Result_With_Error_Code()
        {
            Collection<PSObject> results = CakeHelper.CreatePowershellRunner().Start(new FilePath("../../Scripts/FailingScript.ps1"), new PowershellSettings());

            Assert.True(results != null && results.Count >= 1, "Check Rights");
            Assert.True(results.FirstOrDefault(r => r.BaseObject.ToString().Contains("Cannot find path")) != null);
            Assert.Equal("1", results[0].BaseObject.ToString());
        }


        /*
        [Fact]
        public void Should_Start_Remote_File()
        {
            Collection<PSObject> results = CakeHelper.CreatePowershellRunner().Start(new FilePath("../../Scripts/Test.ps1"), 
                new PowershellSettings()
                {
                    ComputerName = "remote-machine"
                });

            Assert.True((results != null) && (results.Count > 0), "Check Rights");
        }
        */
        #endregion
    }
}
