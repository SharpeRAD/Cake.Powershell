#region Using Statements
    using System;
    using System.IO;
    using System.Collections.ObjectModel;

    using Xunit;

    using System.Management.Automation;
    using System.Management.Automation.Runspaces;

    using Cake.Core;
    using Cake.Core.Diagnostics;
    using Cake.Core.IO;
    using Cake.Powershell;
#endregion



namespace Cake.Powershell.Tests
{
    public class ScriptTests
    {
        [Fact]
        public void Script_Write()
        {
            Collection<PSObject> results = CakeHelper.CreatePowershellRunner().Start("Write-Host", 
                new PowershellSettings().WithArguments(args => args.Append("Testing...")));

            Assert.True((DebugLog.Lines != null) && (DebugLog.Lines.Contains("Testing...")), "Output not written to the powershell host");
        }

        [Fact]
        public void Script_Local()
        {
            Collection<PSObject> results = CakeHelper.CreatePowershellRunner().Start("Get-Service", 
                new PowershellSettings());

            Assert.True((results != null) && (results.Count > 0), "Check Rights");
        }



        /*
        [Fact]
        public void Script_Remote()
        {
            Collection<PSObject> results = CakeHelper.CreatePowershellRunner().Start("Get-Service", 
                new PowershellSettings()
                {
                    ComputerName = "remote-machine"
                });

            Assert.True((results != null) && (results.Count > 0), "Check Rights");
        }
        */
    }
}
