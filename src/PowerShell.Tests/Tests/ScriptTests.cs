#region Using Statements
using System.Collections.ObjectModel;
using System.Management.Automation;

using Xunit;

using Cake.Core;
using Cake.Powershell;
#endregion



namespace Cake.Powershell.Tests
{
    public class ScriptTests
    {
        #region Methods (4)
        [Fact]
        public void Should_Start_Write_Script()
        {
            Collection<PSObject> results = CakeHelper.CreatePowershellRunner().Start("Write-Host", 
                new PowershellSettings().WithArguments(args => args.Append("Testing...")));

            Assert.True((DebugLog.Lines != null) && (DebugLog.Lines.Contains("Testing...")), "Output not written to the powershell host");
        }

        [Fact]
        public void Should_Start_Service_Script()
        {
            Collection<PSObject> results = CakeHelper.CreatePowershellRunner().Start("Get-Service", 
                new PowershellSettings());

            Assert.True((results != null) && (results.Count > 0), "Check Rights");
        }

        [Fact]
        public void Working_Directory_With_Spaces_Should_Properly_Execute()
        {
            Collection<PSObject> results = CakeHelper.CreatePowershellRunner().Start("Write-Host",
                new PowershellSettings().WithArguments(args => args.Append("Testing..."))
                                        .UseWorkingDirectory(@"C:\Path With Spaces\"));

            Assert.True((DebugLog.Lines != null) && (DebugLog.Lines.Contains("Testing...")), "Output not written to the powershell host");
        }
        
        [Fact]
        public void Escapes_Curley_Brackets()
        {
            Collection<PSObject> results = CakeHelper.CreatePowershellRunner().Start("Write-Host '{ blah }'",
                new PowershellSettings());

            Assert.True((DebugLog.Lines != null) && (DebugLog.Lines.Contains("{ blah }")), "Output not written to the powershell host");
        }
        #endregion
    }
}